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

	// the position of this entity in tilespace
	public Vector2Int GroundPosition { get; private set; } = Vector2Int.zero;

	// the floor that the entity is on or above
	public int CurrentFloor { get; private set; } = 0;

	// the position of the player vertically above the floor
	public int VerticalPosition => Mathf.FloorToInt(VerticalPositionF);

	// the floating point position of the player vertically above the floor
	public float VerticalPositionF { get; private set; } = 0.0f;

	// checks if the entity can be given a move input
	public bool CanMove => IsStanding;

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
		Walking
	}

	// the current state of this body
	public EntityBodyState State { get; private set; } = EntityBodyState.Standing;

	// bools to check the state
	public bool IsStanding => State == EntityBodyState.Standing;
	public bool IsWalking => State == EntityBodyState.Walking;

	[SerializeField] private Vector2 m_worldScale = Vector2.one;
	[SerializeField] private bool m_isSolid = false;
	[Tooltip("Tiles per second")]
	[SerializeField] private float m_walkSpeed = 5f;

	private Direction m_currentMoveDir = Direction.None;
	private Direction m_nextMoveDir = Direction.None;
	private float m_moveDelta = 0.0f;
	private Vector2 m_offsetPos = Vector2.zero;

	// moves this body according to the input direction, 
	// returns false if already moving or if input was None or if it failed to move
	public bool MoveInput(Direction dir) {
		if (!CanMove || dir == Direction.None) {
			m_nextMoveDir = dir;
			return false;
		}

		m_moveDelta = 0.0f;
		return CheckAndMove(dir);
	}

	private bool CheckAndMove(Direction dir) {
		// test to see if we can move
		Vector2Int newpos = GroundPosition + dir.ToVector2Int();
		if (TilePhysics.GetTile(newpos, CurrentFloor) == TileType.Wall) {
			return false;
		}

		// set our info
		m_currentMoveDir = dir;
		State = EntityBodyState.Walking;

		return true;
	}

	private void Awake() {
		//TilePhysics.Instance.AddEntity(this);

		GroundPosition = transform.position.RoundToVector2Int();
	}

	//private void OnDestroy() {
	//	if (!TilePhysics.Instance.RemoveEntity(this)) {
	//		Debug.LogError("Failed to remove EntityBody from list, it was not found!");
	//	}
	//}

	private void LateUpdate() {

		// walk
		if (IsWalking) {
			m_moveDelta += m_walkSpeed * Time.deltaTime;

			// reached tile
			if (m_moveDelta > 1.0f) {
				// move ground pos
				GroundPosition += m_currentMoveDir.ToVector2Int();

				// check for movement
				if (m_nextMoveDir != Direction.None) {
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
				if (IsStanding) {
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


		// set position
		Vector3 pos = GroundPosition.ToVector3();
		pos.y += VerticalPositionF;
		transform.position = pos + m_offsetPos.ToVector3() + new Vector3(0.5f, 0.5f, 0f);

	}

}
