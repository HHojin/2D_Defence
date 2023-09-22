using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileManager : MonoBehaviour
{
    public Tilemap tileMap;
    public TileBase[] tileBase = new TileBase[9];
    [SerializeField]
    PolygonCollider2D polygonCollider2D;

    private void Start()
    {
        polygonCollider2D = GetComponentInChildren<PolygonCollider2D>();
    }

    public int GetTileBaseCount() { return tileBase.Length; }

    public void ResetTileMap() { tileMap.ClearAllTiles(); }

    public void DrawTile(int x, int y, int num)
    {
        tileMap.SetTile(new Vector3Int(x, y, 0), tileBase[num]);
    }

    public void SetPolygonCollider(int x, int y)
    {
        polygonCollider2D.points = new[] {new Vector2(x + 5, y + 5), new Vector2(x + 5, -5),
                                        new Vector2(-5, -5), new Vector2(-5, y + 5) };
        polygonCollider2D.SetPath(0, polygonCollider2D.points);
    }
}
