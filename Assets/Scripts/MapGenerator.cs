using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class MapGenerator : MonoBehaviour
{

    [SerializeField] private int gridWidth = 100;
    [SerializeField] private int gridHeight = 100;
    [SerializeField] private float cellSize = 1f;

    public GridXY grid;

    public float magnification = 7.0f; // 4 ~ 20

    float xOffset = 0f; // <- +>
    float yOffset = 0f;

    public UnityEvent<float, float> mapGenerated;

    // test slider
    public Slider sliderMag;
    public Slider sliderXOffset;
    public Slider sliderYOffset;

    private TileManager tileManager;

    private static MapGenerator instance;

    public static MapGenerator Instance()
    {
        return instance;
    }

    private void Awake()
    {
        instance = this;

        grid = new GridXY(gridWidth, gridHeight, cellSize);

        tileManager = gameObject.GetComponent<TileManager>();
    }

    public void GenerateMap()
    {
        tileManager.ResetTileMap();

        magnification = sliderMag.value;
        xOffset = Random.Range(0f, 99999f);
        yOffset = Random.Range(0f, 99999f);

        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                int tileId = GetIdUsingPerlin(x, y);

                grid.SetGridArray(x, y, tileId);
                tileManager.DrawTile(x, y, tileId);

                grid.SetCellArray(x, y);
            }
        }

        grid.UpdateNeighbors();
        GameManager.Instance.MapGenerated(gridWidth, gridHeight);
    }

    int GetIdUsingPerlin(int x, int y)
    {
        float rawPerlin = Mathf.PerlinNoise(
            (x - xOffset) / magnification,
            (y - yOffset) / magnification
        );

        float clampPerlin = Mathf.Clamp(rawPerlin, 0.0f, 1.0f);
        float scalePerlin = clampPerlin * tileManager.GetTileBaseCount();
        //float scalePerlin = clampPerlin * (tileManager.GetTileBaseCount()-5);
        //float scalePerlin = clampPerlin * tileManager.GetTileMapCount();

        if (scalePerlin >= 4) scalePerlin = 4;

        return Mathf.FloorToInt(scalePerlin); // 0 ~ 9
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