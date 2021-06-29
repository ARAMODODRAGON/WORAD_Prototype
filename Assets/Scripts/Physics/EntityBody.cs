using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class EntityBody : MonoBehaviour {

	// the entity attached to this object
	public Entity Entity { get; private set; } = null;

	// the position of this entity in tilespace
	public Vector2Int Position { get; private set; }

	// checks if the entity can be given a move input
	public bool CanMove { get; private set; }

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

	// called when this entity stops moving and lands on something
	public delegate void OnEntityCollisionEvent();

	[SerializeField] private Vector2 m_worldScale;
	[SerializeField] private bool m_isSolid;

	public bool MoveInput(Direction dir) {
		return false;
	}

	private void Awake() {
		//TilePhysics.Instance.AddEntity(this);
		Entity = GetComponent<Entity>();
	}

	//private void OnDestroy() {
	//	if (!TilePhysics.Instance.RemoveEntity(this)) {
	//		Debug.LogError("Failed to remove EntityBody from list, it was not found!");
	//	}
	//}

	private void Update() {

	}

}
