using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilePhysics : ScriptableObject {

	public static Room ActiveRoom { get; set; } = null;

	// returns the room with the given name
	public static Room GetRoom(string name) {
		try {
			return m_rooms[name];
		} catch (KeyNotFoundException) {
			return null;
		}
	}

	// gets the tiletype at the position and floor
	public static TileType GetTile(Vector2Int pos, int floorpos) {
		Floor floor = ActiveRoom.GetFloor(floorpos);
		if (floor) return floor.GetTile(pos);
		return TileType.Floor; // assume floor is default
	}

	// gets the entity at position
	public static EntityBody GetEntity(Vector2Int pos, int floorpos) {
		Floor floor = ActiveRoom.GetFloor(floorpos);
		if (floor) return floor.GetEntityAt(pos);
		return null;
	}

	public static void AddRoom(string name, Room room) => m_rooms.Add(name, room);
	public static bool RemoveRoom(string name) => m_rooms.Remove(name);

	private static Dictionary<string, Room> m_rooms = new Dictionary<string, Room>();

	//public static TilePhysics Instance 
	//	get 
	//		if (!s_instance) s_instance = CreateInstance<TilePhysics>();
	//		return s_instance;
	//	
	//
	//private static TilePhysics s_instance = null;
	//public void AddEntity(EntityBody e) => m_entities.Add(e);
	//public bool RemoveEntity(EntityBody e) => m_entities.Remove(e);
	//public void AddRoom(Room r) => m_rooms.Add(r);
	//public bool RemoveRoom(Room r) => m_rooms.Remove(r);
	//private List<EntityBody> m_entities = new List<EntityBody>();
	//private List<Room> m_rooms = new List<Room>();
}
