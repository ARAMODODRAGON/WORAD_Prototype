using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class EntityBody : MonoBehaviour {

	// the entity attached to this object
	public Entity Entity {
		get {
			if (__entity == null) __entity = GetComponent<Entity>();
			return __entity;
		}
	}
	private Entity __entity = null;

	// callbacks for overlapping something
	public delegate void LandOnEntity(EntityBody body);
	public delegate void LandOnTile(TileType tile);
	public LandOnEntity landOnEntity;
	public LandOnTile landOnTile;

	// the position of this entity in tilespace
	public Vector2Int GroundPosition { get; private set; } = Vector2Int.zero;

	// the floor that the entity is on or above
	public int CurrentFloor {
		get => __currentFloor;
		set {
			if (__currentFloor != value) {
				// remove from current floor
				TilePhysics.ActiveRoom?.GetFloor(__currentFloor)?.__RemoveEntity(this);
				transform.parent = null;
				if (value != -1) {
					// add to new floor
					Floor floor = TilePhysics.ActiveRoom?.GetFloor(value);
					if (floor) {
						floor.__AddEntity(this);
						transform.parent = floor.transform;
					}
				}
				// change value
				__currentFloor = value;
			}
		}
	}
	private int __currentFloor = -1;

	// the position of the player vertically above the floor
	public int VerticalPositionFloored => Mathf.FloorToInt(VerticalPositionF);

	// the position of the player vertically below the floor
	public int VerticalPositionCeiled => Mathf.CeilToInt(VerticalPositionF);

	// the floating point position of the player vertically above the floor
	public float VerticalPositionF { get; private set; } = 0.0f;

	// the ground position + the floored vertical position
	public Vector2Int FlooredTilePosition => GroundPosition + new Vector2Int(0, VerticalPositionFloored);

	// the ground position + the ceiled vertical position
	public Vector2Int CeiledTilePosition => GroundPosition + new Vector2Int(0, VerticalPositionCeiled);

	// checks if the entity can be given a move input
	public bool CanMove => IsStanding;

	// checks if this entity is standing on the ground
	public bool IsGrounded => IsStanding || IsWalking;

	// determines how big 1 tile is
	public Vector2 WorldScale {
		get => m_worldScale;
		set => m_worldScale = value;
	}

	// is this entity treated as a solid when other entities try to move into it?
	public bool IsSolid {
		get => m_isSolid;
		set => m_isSolid = value;
	}

	// body state
	public enum EntityBodyState : byte {
		Standing,
		Walking,
		Floating,
		Falling
	}

	// the current state of this body
	public EntityBodyState State { get; private set; } = EntityBodyState.Standing;

	// bools to check the state
	public bool IsStanding => State == EntityBodyState.Standing;
	public bool IsWalking => State == EntityBodyState.Walking;
	public bool IsFloating => State == EntityBodyState.Floating;
	public bool IsFalling => State == EntityBodyState.Falling;

	// read only
	//[SerializeField] private EntityBodyState currentState = EntityBodyState.Standing;

	// inspector stuff
	[Header("Settings")]
	[SerializeField] private Vector2 m_worldScale = Vector2.one;
	[SerializeField] private bool m_isSolid = false;
	[Header("Movement (Tiles per second)")]
	[SerializeField] private float m_walkSpeed = 5f;
	[SerializeField] private float m_floatSpeed = 2.25f;
	[SerializeField] private float m_fallSpeed = 3.5f;

	// trys to move this entity to another position
	// returns false if did not change positions
	public bool TrySetPosition(Vector2Int newPosition) {
		return TrySetPosition(newPosition, CurrentFloor);
	}

	// trys to move this entity to another position and floor
	// returns false if did not change positions
	public bool TrySetPosition(Vector2Int newPosition, int newFloor) {
		if (newFloor < 0 || newFloor >= TilePhysics.ActiveRoom.CountFloors) return false;

		// get the tile
		TileType tile = GetTile(newPosition, newFloor);

		// check if we cant move there
		if (tile == TileType.Wall) return false;

		// move
		GroundPosition = newPosition;
		CurrentFloor = newFloor;

		return true;
	}

	// moves this body according to the input direction, 
	// returns false if already moving or if input was None or if it failed to move
	public bool MoveInput(Direction dir) {
		// cant move so we need to recored the input direction for next frame
		if (!CanMove || dir == Direction.None) {
			m_nextMoveDir = dir;
			return false;
		}

		m_moveDelta = 0.0f;
		return CheckAndMove(dir);
	}

	// calls the TilePhysics.GetTile function with this entity's given floor
	public TileType GetTile(Vector2Int pos) {
		return GetTile(pos, CurrentFloor);
	}

	// calls the TilePhysics.GetTile function 
	public TileType GetTile(Vector2Int pos, int floorpos) {
		return TilePhysics.GetTile(pos, floorpos);
	}

	// gets first tile below lowestfloor that isnt TileType.Floor or TileType.Wall
	public TileType GetTileAbove(Vector2Int pos, int lowestfloor) {
		TileType tile = TileType.Floor;
		for (int i = lowestfloor; i < TilePhysics.ActiveRoom.CountFloors; i++) {
			tile = GetTile(pos, i);
			if (!tile.IsAny(TileType.Floor, TileType.Wall)) return tile;
		}
		return tile;
	}

	// gets first tile below lowestfloor that isnt TileType.Floor or TileType.Wall
	public TileType GetTileAbove(Vector2Int pos, int lowestfloor, out int floorat) {
		floorat = 0;
		TileType tile = TileType.Floor;
		for (int i = lowestfloor; i < TilePhysics.ActiveRoom.CountFloors; i++) {
			floorat = i;
			tile = GetTile(pos, i);
			if (!tile.IsAny(TileType.Floor, TileType.Wall)) return tile;
		}
		return tile;
	}

	// gets first tile above lowestfloor that isnt TileType.Floor or TileType.Wall
	public TileType GetTileBelow(Vector2Int pos, int highestfloor, out int floorat) {
		floorat = highestfloor;
		TileType tile = TileType.Floor;
		for (int i = highestfloor; i >= 0; i--) {
			floorat = i;
			tile = GetTile(pos, i);
			if (!tile.IsAny(TileType.Floor, TileType.Wall)) return tile;
		}
		return tile;
	}

	// gets first tile above lowestfloor that isnt TileType.Floor or TileType.Wall
	public TileType GetTileBelow(Vector2Int pos, int highestfloor) {
		TileType tile = TileType.Floor;
		for (int i = highestfloor; i >= 0; i--) {
			tile = GetTile(pos, i);
			if (!tile.IsAny(TileType.Floor, TileType.Wall)) return tile;
		}
		return tile;
	}

	private bool CheckAndMove(Direction dir) {
		// test to see if we can move
		Vector2Int newpos = GroundPosition + dir.ToVector2Int();
		if (GetTile(newpos, CurrentFloor) == TileType.Wall) {
			return false;
		}

		// set our info
		m_currentMoveDir = dir;
		State = EntityBodyState.Walking;

		return true;
	}


	// private ///////////////////////////////////////////////////////////////
	private Direction m_currentMoveDir = Direction.None;
	private Direction m_nextMoveDir = Direction.None;
	private float m_moveDelta = 0.0f;
	private Vector2 m_offsetPos = Vector2.zero;
	private int m_lastVerticalPos = 0;
	private bool m_landedOnTile = false;
	private List<EntityBody> m_entityList = new List<EntityBody>();

	private void Start() {
		// grab current groundpos
		GroundPosition = (transform.position - new Vector3(0.5f, 0.5f, 0f)).RoundToVector2Int();

		// get floor, else assume floor 0
		Floor floor = GetComponentInParent<Floor>();
		if (floor) CurrentFloor = floor.FloorNumber;

	}

	private void LateUpdate() {

		// do movement
		DoMovement(Time.deltaTime);

		// set position
		transform.position
			= GroundPosition.ToVector3()
			+ m_offsetPos.ToVector3()
			+ new Vector3(0.5f, 0.5f + VerticalPositionF, 0f);

		//currentState = State;

		int ypos = Mathf.FloorToInt(GroundPosition.y + m_offsetPos.y);
		int floor = CurrentFloor;
		if (Mathf.Abs(VerticalPositionF) > 0.1f) floor += 1;
		Entity.Renderer.sortingOrder = (floor * 1000) + 500 - ypos;

		// stood on new tile, invoke callback
		if (m_landedOnTile == true) {
			m_landedOnTile = false;

			// invoke for tile
			landOnTile?.Invoke(GetTile(GroundPosition));

			// invoke for entities
			if (landOnEntity != null) {
				m_entityList.Clear();
				if (TilePhysics.GetEntities(GroundPosition, CurrentFloor, ref m_entityList)) {
					foreach (EntityBody item in m_entityList) {
						landOnEntity(item);
					}
				}
			}
		}
	}

	private void DoMovement(float delta) {
		// walk
		if (IsWalking)
			DoWalking(delta);

		// floating up
		else if (IsFloating)
			DoFloating(delta);

		// falling down
		else if (IsFalling)
			DoFalling(delta);

		// standing
		else if (IsStanding) {
			// check for another state
			TileType tile = GetTile(GroundPosition);
			if (tile.IsAny(TileType.Updraft, TileType.Fall)) {
				// updraft
				if (tile == TileType.Updraft)
					State = EntityBodyState.Floating;
				// fall
				else if (tile == TileType.Fall)
					State = EntityBodyState.Falling;
			}
		}
	}

	private void DoWalking(float delta) {
		m_moveDelta += m_walkSpeed * delta;

		// reached tile
		if (m_moveDelta > 1.0f) {
			m_landedOnTile = true;

			// move ground pos
			GroundPosition += m_currentMoveDir.ToVector2Int();

			// check what we are standing on
			TileType tile = GetTile(GroundPosition);
			if (tile != TileType.Floor) {
				// updraft
				if (tile == TileType.Updraft)
					State = EntityBodyState.Floating;
				// fall
				else if (tile == TileType.Fall)
					State = EntityBodyState.Falling;
				// invalid, reset
				else State = EntityBodyState.Standing;
			}
			// check for movement
			else if (m_nextMoveDir != Direction.None) {
				// continue moving
				if (m_nextMoveDir == m_currentMoveDir) {
					m_moveDelta -= 1f;
					if (!CheckAndMove(m_nextMoveDir)) State = EntityBodyState.Standing;
				}
				// continue in different direction
				else {
					m_moveDelta = 0f;
					if (!CheckAndMove(m_nextMoveDir)) State = EntityBodyState.Standing;
				}
			}
			// force reset when no held direction
			else State = EntityBodyState.Standing;

			// reset
			if (!IsWalking) {
				m_moveDelta = 0f;
				m_offsetPos = Vector2.zero;
				m_currentMoveDir = Direction.None;
			}
		}
		m_nextMoveDir = Direction.None;

		// set offset
		if (!IsStanding && m_moveDelta <= 1.0f) {
			m_offsetPos = m_currentMoveDir.ToVector2() * m_moveDelta * m_worldScale;
		}
	}

	private void DoFloating(float delta) {
		// move up
		VerticalPositionF += delta * m_floatSpeed;

		// check if standing on another updraft or blockdraft
		TileType tile = GetTileAbove(FlooredTilePosition, CurrentFloor + 1, out int onFloor);
		if (tile.IsAny(TileType.Blockdraft, TileType.Updraft)) {
			// change floors
			CurrentFloor = onFloor;
			GroundPosition += new Vector2Int(0, VerticalPositionFloored);
			VerticalPositionF -= VerticalPositionFloored;
			m_lastVerticalPos = VerticalPositionFloored;

			// lock position and return to standing
			if (tile == TileType.Blockdraft) {
				VerticalPositionF = 0f;
				State = EntityBodyState.Standing;
				m_landedOnTile = true;
				return;
			}
		}

		// if standing on updraft, check for input
		if (VerticalPositionFloored == 0 && tile.IsAny(TileType.Blockdraft, TileType.Updraft)) {
			// check for input and move
			if (m_nextMoveDir != Direction.None) {
				m_landedOnTile = true;
				if (CheckAndMove(m_nextMoveDir))
					VerticalPositionF = 0f;
			}
		}
	}

	private void DoFalling(float delta) {
		// move down
		VerticalPositionF -= delta * m_fallSpeed;

		// check if standing on another fall or blockdraft
		TileType tile = GetTileBelow(CeiledTilePosition, CurrentFloor - 1, out int onFloor);
		if (tile.IsAny(TileType.Blockdraft, TileType.Fall)) {
			// change floors
			CurrentFloor = onFloor;
			GroundPosition += new Vector2Int(0, VerticalPositionCeiled);
			VerticalPositionF -= VerticalPositionCeiled;
			m_lastVerticalPos = VerticalPositionCeiled;

			// lock position and return to standing
			if (tile == TileType.Blockdraft) {
				VerticalPositionF = 0f;
				State = EntityBodyState.Standing;
				m_landedOnTile = true;
				return;
			}
		}

		// if standing on updraft, check for input
		if (VerticalPositionCeiled == 0 && tile.IsAny(TileType.Blockdraft, TileType.Fall)) {
			// check for input and move
			if (m_nextMoveDir != Direction.None) {
				m_landedOnTile = true;
				if (CheckAndMove(m_nextMoveDir))
					VerticalPositionF = 0f;
			}
		}

	}

}
