using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Tilemaps;

//#if UNITY_EDITOR
//public class CreateLevelScript {
//	
//	[MenuItem("GameObject/2D Object/Entity")]
//	public static void CreateEntity(MenuCommand menuCommand) {
//		// create
//		GameObject go = new GameObject("Entity");
//		go.AddComponent<SpriteRenderer>();
//		go.AddComponent<EntityBody>();
//
//		// attach to context
//		GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
//
//		// Register the creation in the undo system
//		Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
//		Selection.activeObject = go;
//	}
//
//	[MenuItem("GameObject/2D Object/Room")]
//	public static void CreateRoom(MenuCommand menuCommand) {
//		// create
//		GameObject go = new GameObject("Room");
//		go.AddComponent<Room>();
//		go.AddComponent<Grid>();
//
//		// attach to context
//		GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
//
//		// Register the creation in the undo system
//		Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
//		Selection.activeObject = go;
//	}
//
//	[MenuItem("GameObject/2D Object/Floor")]
//	public static void CreateFloor(MenuCommand menuCommand) {
//		// create
//		GameObject go = new GameObject("Floor");
//		Floor f = go.AddComponent<Floor>();
//
//		GameObject visual = new GameObject("Visuals");
//		visual.AddComponent<Tilemap>();
//		var rend0 = visual.AddComponent<TilemapRenderer>();
//		f.__SetVisualMap(rend0);
//		GameObjectUtility.SetParentAndAlign(visual, go);
//		rend0.sortingLayerID = SortingLayer.NameToID("Visuals");
//
//		GameObject collision = new GameObject("Collision");
//		f.__SetCollisionMap(collision.AddComponent<Tilemap>());
//		var rend1 = collision.AddComponent<TilemapRenderer>();
//		GameObjectUtility.SetParentAndAlign(collision, go);
//		rend1.sortingLayerID = SortingLayer.NameToID("Collision");
//
//		// attach to context
//		GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
//
//		// Register the creation in the undo system
//		Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
//		Selection.activeObject = go;
//	}
//
//}
//#endif
//