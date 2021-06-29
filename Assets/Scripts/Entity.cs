using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EntityBody))]
public class Entity : MonoBehaviour {

	public EntityBody Body { get; private set; } = null;

	public Vector2Int Position {
		get => Body.Position;
		set => Body.Position = value;
	}

	protected virtual void Awake() {
		if (!Body) Body = gameObject.AddComponent<EntityBody>();
		else Body = GetComponent<EntityBody>();
	}

	
}