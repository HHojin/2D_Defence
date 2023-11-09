using UnityEngine;

//public class GridXY<GridObject>
public class GridXY
{
    private int width;
    private int height;
    private float cellSize = 1f;
    private int cellHeight;
    private Vector3 originPosition;

    private int[,] terrainHeightArray; // Terrain Height
    public Cell[,,] waterArray; // Water simulation
    private PlacedObjectData placedObjectData;

    //public GridXY(int width, int height, float cellSize, Vector3 originPosition, Func<GridXY<GridObject>, int, int, GridObject> createGridObject)
    public GridXY(int width, int height, float cellSize, int cellHeight)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.cellHeight = cellHeight;

        //terrainHeightArray = new GridObject[width, height];
        terrainHeightArray = new int[width, height];
        waterArray = new Cell[width, height, cellHeight];
        placedObjectData = new();

        for (int x = 0; x < terrainHeightArray.GetLength(0); x++)
        {
            for (int y = 0; y < terrainHeightArray.GetLength(1); y++)
            {
                //terrainHeightArray[x, y] = createGridObject(this, x, y);
                //Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white, 10f);
                //Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white, 10f);
            }
        }

        //Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, 10f);
        //Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, 10f);
    }

    public int GetWidth() => width;
    public int GetHeight() => height;
    public float GetCellSize() => cellSize;
    public int GetWaterArray() => cellHeight;
    //public Vector3 GetWorldPosition(int x, int y) { return new Vector3(x, y, 0) * cellSize + originPosition; }
    public void GetXY(Vector3 worldPosition, out int x, out int y)
    {
        x = Mathf.FloorToInt((worldPosition - originPosition).x / cellSize);
        y = Mathf.FloorToInt((worldPosition - originPosition).y / cellSize);
    }
    public void SetTerrainHeightArray(int x, int y, int value)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            terrainHeightArray[x, y] = value;
        }
    }

    public int GetTerrainHeightArray(int x, int y) => terrainHeightArray[x, y];

    public void SetWaterArray(int x, int y, int z, CellType type)
    {
        waterArray[x, y, z] = new Cell(x, y, z);
        waterArray[x, y, z].Type = type;
    }

    public int GetWaterArray(int x, int y)
    {
        for (int z = cellHeight - 1; z >= 0; z--)
        {
            if (waterArray[x, y, z].Liquid >= 0.01f)
                return z;
        }

        return 0;
    }

    public ref int[,] GetTerrainHeightArray() => ref terrainHeightArray;

    public ref Cell[,,] GetWaterArrayRef() => ref waterArray;

    public ref PlacedObjectData GetPlacedObjectDataRef() => ref placedObjectData;

    public void UpdateNeighbors()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                for (int z = 0; z < cellHeight; z++)
                {
                    if (x > 0) waterArray[x, y, z].Left = waterArray[x - 1, y, z];
                    if (x < width - 2) waterArray[x, y, z].Right = waterArray[x + 1, y, z];
                    if (y > 0) waterArray[x, y, z].Down = waterArray[x, y - 1, z];
                    if (y < height - 2) waterArray[x, y, z].Up = waterArray[x, y + 1, z];
                    if (z > 0) waterArray[x, y, z].Bottom = waterArray[x, y, z - 1];
                    if (z < cellHeight - 2) waterArray[x, y, z].Top = waterArray[x, y, z + 1];
                }
            }
        }
    }
    /*
    public void SetGridObject(int x, int y, GridObject value)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            gridArray[x, y] = value;
            TriggerGridObjectChanged(x, y);
        }
    }

    public void TriggerGridObjectChanged(int x, int y)
    {
        OnGridObjectChanged?.Invoke(this, new OnGridObjectChangedEventArgs { x = x, y = y });
    }

    public void SetGridObject(Vector3 worldPosition, GridObject value)
    {
        GetXY(worldPosition, out int x, out int z);
        SetGridObject(x, z, value);
    }

    public GridObject GetGridObject(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            return gridArray[x, y];
        }
        else
        {
            return default(GridObject);
        }
    }

    public GridObject GetGridObject(Vector3 worldPosition)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        return GetGridObject(x, y);
    }

    public Vector2Int ValidateGridPosition(Vector2Int gridPosition)
    {
        return new Vector2Int(
            Mathf.Clamp(gridPosition.x, 0, width - 1),
            Mathf.Clamp(gridPosition.y, 0, height - 1)
        );
    }

    public bool IsValidGridPosition(Vector2Int gridPosition)
    {
        int x = gridPosition.x;
        int y = gridPosition.y;

        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    */
}