using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileManager : Singleton<TileManager>
{
    public GridXY grid;

    /*
     * 0 : map
     * 1 : water
     */

    [Header("Map")]
    public Tilemap map;
    public TileBase[] mapTileBase = new TileBase[5];

    [Header("Water")]
    public Tilemap water;
    public TileBase[] waterTileBase = new TileBase[5];

    [Header("Grid Info")]
    [SerializeField] private int gridWidth = 140;
    [SerializeField] private int gridHeight = 90;
    [SerializeField] private float cellSize = 1f;
    [SerializeField] private int cellHeight = 5;

    protected override void Awake()
    {
        grid = new GridXY(gridWidth, gridHeight, cellSize, cellHeight);
    }

    /*
    public bool GetTile(int type, Vector3Int pos)
    {
        switch (type)
        {
            case 0:
                return map.GetTile(pos) != null;
            case 1:
                return water.GetTile(pos) != null;
        }

        return false;
    }
    */

    public int GetTileBaseCount(int type)
    {
        switch (type)
        {
            case 0:
                return mapTileBase.Count();
            case 1:
                return 1;
        }

        return 0;
    }

    public void ResetTileMap(int type)
    {
        switch (type)
        {
            case 0:
                map.ClearAllTiles();
                break;
            case 1:
                water.ClearAllTiles();
                break;
        }
    }

    public void DrawTile(int x, int y, int type, int tileBaseIdx)
    {
        switch (type)
        {
            case 0:
                map.SetTile(new Vector3Int(x, y, 0), mapTileBase[tileBaseIdx]);
                break;
            case 1:
                if(tileBaseIdx == -1) water.SetTile(new Vector3Int(x, y, 0), null);
                else water.SetTile(new Vector3Int(x, y, 0), waterTileBase[tileBaseIdx]);
                break;
        }
    }

    //tmp
    public TMP_Text terrainHeight;

    public void GridHeight()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (mouseWorldPos.x >= 0 && mouseWorldPos.y >= 0
                && mouseWorldPos.x <= map.size.x && mouseWorldPos.y <= map.size.y)
            {
                var tmp = map.WorldToCell(mouseWorldPos);
                terrainHeight.text = "[" + tmp.x + "," + tmp.y + "]"
                                    + grid.GetGridArray(tmp.x, tmp.y) + " "
                                    + grid.GetCellArray(tmp.x, tmp.y);
            }
        }
    }
}