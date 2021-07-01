using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EntityBody))]
public abstract class Entity : MonoBehaviour {

	public EntityBody Body { get; private set; } = null;
	public Renderer Renderer {
		get {
			if (!__renderer) __renderer = GetComponent<Renderer>();
			return __renderer;
		}
	}
	private Renderer __renderer = null;

	protected virtual void Awake() {
		if (!Body) Body = GetComponent<EntityBody>();
		else if (!Body) Body = gameObject.AddComponent<EntityBody>();
	}

}