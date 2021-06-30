using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction : byte {
	None,
	Up,
	Down,
	Left,
	Right
}

public static class DirectionMath {
	public static Vector2 ToVector2(Direction dir) {
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
	public static Vector2Int ToVector2Int(Direction dir) {
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