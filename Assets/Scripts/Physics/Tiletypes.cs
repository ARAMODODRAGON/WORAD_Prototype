using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum TileType : byte {
	Floor,
	Wall,
	Fall,
	Updraft,
	Blockdraft
}

[CreateAssetMenu(fileName = "Updraft Tile", menuName = "Collision Tiles/Updraft Tile")]
public class UpdraftTile : Tile { }

[CreateAssetMenu(fileName = "Wall Tile", menuName = "Collision Tiles/Wall Tile")]
public class WallTile : Tile { }

[CreateAssetMenu(fileName = "Fall Tile", menuName = "Collision Tiles/Fall Tile")]
public class FallTile : Tile { }

[CreateAssetMenu(fileName = "Blockdraft Tile", menuName = "Collision Tiles/Blockdraft Tile")]
public class BlockdraftTile : Tile { }
