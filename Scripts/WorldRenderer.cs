using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WorldRenderer : MonoBehaviour
{
    // References to different Tilemaps
    public Tilemap groundTilemap;      // Tilemap for ground tiles
    public Tilemap backgroundTilemap;  // Tilemap for background tiles

    // Set a ground tile at a specific position (x, y)
    public void SetGroundTile(int x, int y, TileBase tile)
    {
        // Convert world position to cell position and set the tile
        groundTilemap.SetTile(groundTilemap.WorldToCell(new Vector3Int(x, y, 0)), tile);
    }

    // Set a background tile at a specific position (x, y)
    public void SetBackgroundTile(int x, int y, TileBase tile)
    {
        // Convert world position to cell position and set the tile
        backgroundTilemap.SetTile(backgroundTilemap.WorldToCell(new Vector3Int(x, y, 0)), tile);
    }

    // Clear all tiles from the ground and background tilemaps
    public void ClearGroundTilemap()
    {
        groundTilemap.ClearAllTiles();       // Clear ground tiles
        backgroundTilemap.ClearAllTiles();   // Clear background tiles
    }    
}
