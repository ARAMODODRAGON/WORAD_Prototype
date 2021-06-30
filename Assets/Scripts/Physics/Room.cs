using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Room : MonoBehaviour {

	public string RoomName => m_roomName;
	public int CountFloors => m_floors.Count;
	public Floor GetFloor(int index) => m_floors[index];

	[SerializeField] private string m_roomName;

	private List<Floor> m_floors = new List<Floor>();

	private void Awake() {
		//TilePhysics.Instance.AddRoom(this);

		// get all floors in children
		foreach (Transform t in transform) {
			Floor f = t.GetComponent<Floor>();
			if (f) m_floors.Add(f);
			f.gameObject.hideFlags |= HideFlags.NotEditable;
		}
		m_floors.Reverse();

	}

	private void Start() {
		TilePhysics.ActiveRoom = this;	
	}

	//private void OnDestroy() {
	//	if (!TilePhysics.Instance.RemoveRoom(this)) {
	//		Debug.LogError("Failed to remove Room from list, it was not found!");
	//	}
	//}
}