using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Entity {

	// get/set silver keys 
	public int SilverKeys {
		get => (m_silverKeys ? m_silverKeys.value : 0);
		set { if (m_silverKeys) m_silverKeys.value = value; }
	}

	// get/set golden keys 
	public int GoldenKeys {
		get => (m_goldenKeys ? m_goldenKeys.value : 0);
		set { if (m_goldenKeys) m_goldenKeys.value = value; }
	}

	// keys
	[SerializeField] private IntValue m_silverKeys;
	[SerializeField] private IntValue m_goldenKeys;

	// private ///////////////////////////////

	private List<EntityBody> m_entities = new List<EntityBody>();

	private void Start() {
		m_silverKeys.value = 0;
		m_goldenKeys.value = 0;
	}

	private void Update() {

		// get input
		bool up = Input.GetKey(KeyCode.UpArrow);
		bool down = Input.GetKey(KeyCode.DownArrow);
		bool left = Input.GetKey(KeyCode.LeftArrow);
		bool right = Input.GetKey(KeyCode.RightArrow);
		bool pressed = Input.GetKeyDown(KeyCode.C);

		// get dir
		Direction dir = Direction.None;
		if (up != down) {
			if (up) dir = Direction.Up;
			if (down) dir = Direction.Down;
		} else if (left != right) {
			if (right) dir = Direction.Right;
			if (left) dir = Direction.Left;
		}

		// try move
		bool didmove = Body.MoveInput(dir);

		// check if an entity blocked us
		if (!didmove && Body.IsStanding) {

			m_entities.Clear();
			if (!TilePhysics.GetEntities(Body.GroundPosition + dir.ToVector2Int(), Body.CurrentFloor, ref m_entities)) return;

			foreach (EntityBody eb in m_entities) {
				// is box 
				if (eb.Entity is BoxEntity box) {
					// try to push
					box.TryMove(dir);
				}

				// is door
				if (eb.Entity is DoorEntity door) {
					// try to open door if locked
					if (door.Locked && pressed) {
						door.TryKeyUnlock(m_silverKeys, m_goldenKeys);
					}

					// enter door
					else if (!door.Locked && pressed) {
						DoorEntity other = door.ConnectingDoor;
						if (other) {
							// try to move to the space in front of this door
							Body.TrySetPosition(
								other.Body.GroundPosition + Direction.Down.ToVector2Int(),
								other.Body.CurrentFloor
							);
						}
					}
				}
			}
		}

	}
}
