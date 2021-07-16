using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorEntity : Entity {

	public enum DoorType : byte {
		Unlocked,
		SilverLocked,
		GoldenLocked,
		ButtonLocked
	}

	[Header("Settings")]
	[SerializeField] private DoorType m_doorType = DoorType.Unlocked;
	[SerializeField] private DoorEntity m_connectingDoor;

	[Header("Sprites")]
	[SerializeField] private Sprite m_unlockedDoorSprite;
	[SerializeField] private Sprite m_silverLockedDoorSprite;
	[SerializeField] private Sprite m_goldenLockedDoorSprite;
	[SerializeField] private Sprite m_buttonLockedDoorSprite;

	public DoorEntity ConnectingDoor => m_connectingDoor;

	public bool Locked {
		get => m_locked;
		private set {
			// became locked
			if (!value && value != m_locked) {
				m_spr.sprite = m_unlockedDoorSprite;
			}
			// became unlocked
			else if (value && value != m_locked) {
				switch (m_doorType) {
					case DoorType.SilverLocked:
						m_spr.sprite = m_silverLockedDoorSprite;
						break;
					case DoorType.GoldenLocked:
						m_spr.sprite = m_goldenLockedDoorSprite;
						break;
					case DoorType.ButtonLocked:
						m_spr.sprite = m_buttonLockedDoorSprite;
						break;
					default: break;
				}
			}
			m_locked = value;
		}
	}
	private bool m_locked = false;

	// tries to unlock the door using a key,
	// returns false if no keys or is already unlocked
	public bool TryKeyUnlock(IntValue silverKeys, IntValue goldenKeys) {
		switch (m_doorType) {
			case DoorType.SilverLocked:
				if (silverKeys && silverKeys.value > 0) {
					silverKeys.value--;
					Locked = false;
					if (ConnectingDoor && ConnectingDoor.m_doorType == m_doorType
						|| ConnectingDoor.m_doorType == DoorType.ButtonLocked)
						ConnectingDoor.Locked = false;
					return true;
				}
				break;
			case DoorType.GoldenLocked:
				if (goldenKeys && goldenKeys.value > 0) {
					goldenKeys.value--;
					Locked = false;
					if (ConnectingDoor && ConnectingDoor.m_doorType == m_doorType
						|| ConnectingDoor.m_doorType == DoorType.ButtonLocked)
						ConnectingDoor.Locked = false;
					return true;
				}
				break;
			default: break;
		}
		return false;
	}

	// unlocks the door if its locked by a button
	// returns false if this is not a button locked door
	public bool UnlockDoor() {
		if (m_doorType != DoorType.ButtonLocked) return false;
		Locked = false;
		if (ConnectingDoor && ConnectingDoor.m_doorType == m_doorType)
			ConnectingDoor.Locked = false;
		return true;
	}

	// locks the door if its locked by a button
	// returns false if this is not a button locked door
	public bool LockDoor() {
		if (m_doorType != DoorType.ButtonLocked) return false;
		Locked = true;
		if (ConnectingDoor && ConnectingDoor.m_doorType == m_doorType)
			ConnectingDoor.Locked = true;
		return true;
	}

	// private /////////////////////////////////////

	private SpriteRenderer m_spr = null;

	private void Start() {

		m_spr = GetComponent<SpriteRenderer>();
		if (!m_spr) m_spr = gameObject.AddComponent<SpriteRenderer>();

		switch (m_doorType) {
			case DoorType.Unlocked:
				m_spr.sprite = m_unlockedDoorSprite;
				m_locked = false;
				break;
			case DoorType.SilverLocked:
				m_spr.sprite = m_silverLockedDoorSprite;
				m_locked = true;
				break;
			case DoorType.GoldenLocked:
				m_spr.sprite = m_goldenLockedDoorSprite;
				m_locked = true;
				break;
			case DoorType.ButtonLocked:
				m_spr.sprite = m_buttonLockedDoorSprite;
				m_locked = true;
				break;
			default:
				break;
		}
	}

}
