using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilePhysics : ScriptableObject {

	public static TilePhysics Instance {
		get {
			if (!s_instance) s_instance = CreateInstance<TilePhysics>();
			return s_instance;
		}
	}
	private static TilePhysics s_instance = null;





	public void AddEntity(EntityBody e) => m_entities.Add(e);
	public bool RemoveEntity(EntityBody e) => m_entities.Remove(e);
	public void AddCollisionMap(CollisionMap cm) => m_collisionmaps.Add(cm);
	public bool RemoveCollisionMap(CollisionMap cm) => m_collisionmaps.Remove(cm);
	private List<EntityBody> m_entities = new List<EntityBody>();
	private List<CollisionMap> m_collisionmaps = new List<CollisionMap>();

}
