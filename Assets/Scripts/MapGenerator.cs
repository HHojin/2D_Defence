using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using System.Linq;
using UnityEngine.Events;

public class MapGenerator : MonoBehaviour
{
    public PolygonCollider2D polygonCollider2D;

    private int gridWidth;
    private int gridHeight;
    private int cellHeight;

    public float magnification = 7.0f; // 4 ~ 20

    float xOffset = 0f; // <- +>
    float yOffset = 0f;

    // test slider
    public Slider sliderMag;
    public Slider sliderXOffset;
    public Slider sliderYOffset;

    [HideInInspector] public UnityEvent<float, float> GeneratedMap;

    public void GenerateMap()
    {
        TileManager.Instance.ResetTileMap(0);

        magnification = sliderMag.value;
        xOffset = Random.Range(0f, 99999f);
        yOffset = Random.Range(0f, 99999f);

        gridWidth = TileManager.Instance.grid.GetWidth();
        gridHeight = TileManager.Instance.grid.GetHeight();
        cellHeight = TileManager.Instance.grid.GetCellHeight();

        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                int tileId = GetIdUsingPerlin(x, y);

                for (int z = cellHeight - 1; z >= tileId; z--)
                {
                    TileManager.Instance.grid.SetCellArray(x, y, z, CellType.Blank);
                }

                for (int z = tileId - 1; z >= 0; z--)
                {
                    TileManager.Instance.grid.SetCellArray(x, y, z, CellType.Solid);
                }

                TileManager.Instance.grid.SetGridArray(x, y, tileId);
                TileManager.Instance.DrawTile(x, y, 0, tileId);
            }
        }

        SetPolygonCollider(gridWidth, gridHeight);
        TileManager.Instance.grid.UpdateNeighbors();

        GameManager.Instance.MapGenerated(gridWidth, gridHeight);
    }

    public void SetPolygonCollider(float x, float y)
    {
        polygonCollider2D.points = new[] {new Vector2(x + 5, y + 5), new Vector2(x + 5, -5),
                                        new Vector2(-5, -5), new Vector2(-5, y + 5) };
        polygonCollider2D.SetPath(0, polygonCollider2D.points);
    }

    int GetIdUsingPerlin(int x, int y)
    {
        float rawPerlin = Mathf.PerlinNoise(
            (x - xOffset) / magnification,
            (y - yOffset) / magnification
        );

        float clampPerlin = Mathf.Clamp(rawPerlin, 0.0f, 1.0f);
        float scalePerlin = clampPerlin * TileManager.Instance.mapTileBase.Count();
        //float scalePerlin = clampPerlin * (tileManager.GetTileBaseCount()-5);
        //float scalePerlin = clampPerlin * tileManager.GetTileMapCount();

        if (scalePerlin >= 4) scalePerlin = 4;

        return Mathf.FloorToInt(scalePerlin); // 0 ~ 4
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            TileManager.Instance.GridHeight();
        }
    }
}


/*
public class GridObject
{
    private GridXY<GridObject> grid;
    private int x;
    private int y;
    private int value;
    public PlacedObject placedObject;

    public GridObject(GridXY<GridObject> grid, int x, int y, int value)
    {
        this.grid = grid;
        this.x = x;
        this.y = y;
        this.value = value;
        placedObject = null;
    }

    public override string ToString()
    {
        return x + ", " + y + "\n" + placedObject;
    }

    public void TriggerGridObjectChanged()
    {
        grid.TriggerGridObjectChanged(x, y);
    }

    public void SetPlacedObject(PlacedObject placedObject)
    {
        this.placedObject = placedObject;
        TriggerGridObjectChanged();
    }

    public void ClearPlacedObject()
    {
        placedObject = null;
        TriggerGridObjectChanged();
    }

    public PlacedObject GetPlacedObject()
    {
        return placedObject;
    }

    public bool CanBuild()
    {
        return placedObject == null;
    }
}
*/