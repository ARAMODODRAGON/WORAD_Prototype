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

}
