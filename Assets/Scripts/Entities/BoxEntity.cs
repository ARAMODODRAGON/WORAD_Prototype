using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxEntity : Entity {

	private void Start() {
		Body.IsSolid = true;
	}

	// tries to move the box in a direction
	public bool TryMove(Direction dir) {
		TileType tile = Body.GetTile(Body.GroundPosition + dir.ToVector2Int());
		if (tile == TileType.BoxMoveable) return Body.MoveInput(dir);
		return false;
	}

}
