using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EntityBody))]
public class Entity : MonoBehaviour {

	public EntityBody Body { get; private set; } = null;

	protected virtual void Awake() {
		if (!Body) Body = gameObject.AddComponent<EntityBody>();
		else Body = GetComponent<EntityBody>();
	}

	
}