using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickupEntity : Entity {
	
	public enum ItemType : byte {
		None,
		GoldenKey,
		SilverKey
	}

	[SerializeField] private ItemType m_itemType;


	private const int UPDATE_RATE = 4;

	private int m_currentRate = 0;
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

				// is player
				if (eb.Entity is PlayerController pc) {
					switch (m_itemType) {
						case ItemType.GoldenKey:
							pc.GoldenKeys++;
							break;
						case ItemType.SilverKey:
							pc.SilverKeys++;
							break;
						default: Debug.LogError("Invalid item type"); return;
					}

					gameObject.SetActive(false);

					return;
				}

			}
		}
	}

}
