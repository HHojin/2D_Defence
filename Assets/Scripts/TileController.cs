using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileController : MonoBehaviour
{
    public Tilemap tileMap;
    public TileBase[] tileBase = new TileBase[9];

    public int GetTileBaseCount() { return tileBase.Length; }

    public void ResetTileMap() { tileMap.ClearAllTiles(); }

    public void DrawTile(int x, int y, int num)
    {
        tileMap.SetTile(new Vector3Int(x, y, 0), tileBase[num]);
    }
}
