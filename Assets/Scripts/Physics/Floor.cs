using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public sealed class Floor : MonoBehaviour {

	public Tilemap CollisionMap => m_collisionMap;
	public int EntityCount => m_entities.Count;
	public EntityBody GetEntity(int index) => m_entities[index];

	public TileType GetTile(Vector2Int pos) {
		TileBase tile = CollisionMap.GetTile(pos.ToVector3Int());

		if (tile is SurfaceTile) return TileType.Floor;
		if (tile is WallTile) return TileType.Wall;
		if (tile is FallTile) return TileType.Fall;
		if (tile is UpdraftTile) return TileType.Updraft;
		return TileType.None;
	}

	[SerializeField] private Tilemap m_collisionMap = null;

	private List<EntityBody> m_entities = new List<EntityBody>();

	private void Awake() {
		// hide it because we only need the data stored within it
		//if (m_collisionMap) m_collisionMap.gameObject.SetActive(false);

		// get all entities
		foreach (Transform t in transform) {
			EntityBody e = GetComponentInChildren<EntityBody>();
			if (e) m_entities.Add(e);
		}
	}

}
