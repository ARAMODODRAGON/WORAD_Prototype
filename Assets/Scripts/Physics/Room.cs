using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Room : MonoBehaviour {

	public string RoomName { get; private set; } = null;
	public int CountFloors => m_floors.Count;
	public Floor GetFloor(int index) => (index >= 0 && index < CountFloors ? m_floors[index] : null);

	private List<Floor> m_floors = new List<Floor>();

	private void Awake() {
		RoomName = gameObject.name;

		// get all floors in children
		foreach (Transform t in transform) {
			if (!t.gameObject.activeSelf) continue;
			Floor f = t.GetComponent<Floor>();
			if (f) m_floors.Add(f);
			f.gameObject.hideFlags |= HideFlags.NotEditable;
		}
		m_floors.Reverse();

		// set their FloorNumbers
		for (int i = 0; i < m_floors.Count; i++) {
			m_floors[i].__SetFloorNumber(i);
		}

		TilePhysics.AddRoom(RoomName, this);
		TilePhysics.ActiveRoom = this;
	}

	private void OnDestroy() {
		TilePhysics.RemoveRoom(RoomName);
	}

}