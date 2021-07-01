using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Entity {

	// Update is called once per frame
	void Update() {

		// get input
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
		
		// try move
		bool didmove = Body.MoveInput(dir);

		// check if an entity blocked us
		if (!didmove) {
			EntityBody eb = TilePhysics.GetEntity(Body.GroundPosition + dir.ToVector2Int(), Body.CurrentFloor);
			if (eb) {
				if (eb.Entity is BoxEntity) {
					eb.MoveInput(dir);	
				}
			}
		}

	}
}
