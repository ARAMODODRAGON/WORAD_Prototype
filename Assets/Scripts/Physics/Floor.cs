using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public sealed class Floor : MonoBehaviour {

	public Tilemap CollisionMap => m_collisionMap;
	public TilemapRenderer VisualMap => m_visualMap;
	public int EntityCount => m_entities.Count;
	public EntityBody GetEntity(int index) => m_entities[index];
	public int FloorNumber { get; private set; } = -1;

	public TileType GetTile(Vector2Int pos) {
		TileBase tile = CollisionMap.GetTile(pos.ToVector3Int());

		if (tile is BlockdraftTile) return TileType.Blockdraft;
		if (tile is WallTile) return TileType.Wall;
		if (tile is FallTile) return TileType.Fall;
		if (tile is UpdraftTile) return TileType.Updraft;

		// get solid entity as tiletype wall
		for (int i = 0; i < EntityCount; i++) {
			EntityBody eb = GetEntity(i);
			if (eb.enabled && eb.IsSolid && eb.IsGrounded && eb.GroundPosition == pos)
				return TileType.Wall;
		}

		return TileType.Floor;
	}

	public EntityBody GetEntityAt(Vector2Int pos) {
		for (int i = 0; i < EntityCount; i++) {
			EntityBody eb = GetEntity(i);
			if (eb.enabled && eb.IsGrounded && eb.GroundPosition == pos)
				return eb;
		}
		return null;
	}

	[SerializeField] private Tilemap m_collisionMap = null;
	[SerializeField] private TilemapRenderer m_visualMap = null;

	private List<EntityBody> m_entities = new List<EntityBody>();

	private void Awake() {
		// hide it because we only need the data stored within it
		if (m_collisionMap) m_collisionMap.gameObject.SetActive(false);
	
		// get all entities
		//foreach (Transform t in transform) {
		//	EntityBody e = GetComponentInChildren<EntityBody>();
		//	if (e) m_entities.Add(e);
		//}
	}

	public void __SetFloorNumber(int index) {
		FloorNumber = index;
		m_visualMap.sortingOrder = index * 2;
	}
	public void __AddEntity(EntityBody entity) => m_entities.Add(entity);
	public void __RemoveEntity(EntityBody entity) => m_entities.Remove(entity);

}
