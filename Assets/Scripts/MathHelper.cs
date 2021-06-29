using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MathHelper {
	public static Vector3Int ToVector3Int(this Vector2Int self) {
		return new Vector3Int(self.x, self.y, 0);
	}
}
