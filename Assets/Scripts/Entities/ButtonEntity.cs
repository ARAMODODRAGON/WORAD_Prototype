using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonEntity : Entity {

	[SerializeField] private DoorEntity m_doorToOpen;
	[SerializeField] private bool m_reverseLock;

	private const int UPDATE_RATE = 4;

	private int m_currentRate = 0;
	private bool m_pressedState = false;
	private List<EntityBody> m_entities = new List<EntityBody>();

	private void Start() {
		m_currentRate = Random.Range(0, UPDATE_RATE);
	}

	private void Update() {
		if (++m_currentRate >= UPDATE_RATE) {
			m_currentRate -= UPDATE_RATE;

			m_entities.Clear();
			if (!TilePhysics.GetEntities(Body.GroundPosition, Body.CurrentFloor, ref m_entities)) return;

			for (int i = 0; i < m_entities.Count; i++) {
				EntityBody eb = m_entities[i];

				// is player or box
				if (eb.Entity is PlayerController) {
					TogglePress(true);
					return;
				}
				if (eb.Entity is BoxEntity) {
					TogglePress(true);
					return;
				}

				// not being stepped on
				else TogglePress(false);
			}
		}
	}

	private void TogglePress(bool state) {
		if (state != m_pressedState) {
			// check if we should open or close based off the reverse button
			bool openDoor = (!m_reverseLock ? state : !state);
			bool success = false;

			// open
			if (openDoor)
				success = m_doorToOpen.UnlockDoor();

			// close
			else
				success = m_doorToOpen.LockDoor();

			// changed
			if (success) m_pressedState = state;
		}
	}

}
