using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MathHelper {

	public static Vector3Int ToVector3Int(this Vector2Int self) {
		return new Vector3Int(self.x, self.y, 0);
	}

	public static Vector3 ToVector3(this Vector2Int self) {
		return new Vector3(self.x, self.y, 0.0f);
	}

	public static Vector2Int RoundToVector2Int(this Vector3 self) {
		return new Vector2Int(Mathf.RoundToInt(self.x), Mathf.RoundToInt(self.y));
	}

	public static Vector3 ToVector3(this Vector2 self) {
		return new Vector3(self.x, self.y, 0.0f);
	}

	public static Vector2 ToVector2(this Direction dir) {
		Vector2 v = Vector2.zero;
		switch (dir) {
			case Direction.None: break;
			case Direction.Up: v.y = 1.0f; break;
			case Direction.Down: v.y = -1.0f; break;
			case Direction.Left: v.x = -1.0f; break;
			case Direction.Right: v.x = 1.0f; break;
			default: Debug.LogError("Invalid direction"); break;
		}
		return v;
	}
	public static Vector2Int ToVector2Int(this Direction dir) {
		Vector2Int v = Vector2Int.zero;
		switch (dir) {
			case Direction.None: break;
			case Direction.Up: v.y = 1; break;
			case Direction.Down: v.y = -1; break;
			case Direction.Left: v.x = -1; break;
			case Direction.Right: v.x = 1; break;
			default: Debug.LogError("Invalid direction"); break;
		}
		return v;
	}
}
