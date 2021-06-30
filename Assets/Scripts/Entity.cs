using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EntityBody))]
public class Entity : MonoBehaviour {

	public EntityBody Body { get; private set; } = null;

	protected virtual void Awake() {
		if (!Body) Body = GetComponent<EntityBody>();
		else if (!Body) Body = gameObject.AddComponent<EntityBody>();
	}

	private void Update() {
		bool up = Input.GetKey(KeyCode.UpArrow);
		bool down = Input.GetKey(KeyCode.DownArrow);
		bool left = Input.GetKey(KeyCode.LeftArrow);
		bool right = Input.GetKey(KeyCode.RightArrow);
		Direction dir = Direction.None;
		if (up != down) {
			if (up) dir = Direction.Up;
			if (down) dir = Direction.Down;
		} else if (left != right) {
			if (right) dir = Direction.Right;
			if (left) dir = Direction.Left;
		}
		Body.MoveInput(dir);
	}

}