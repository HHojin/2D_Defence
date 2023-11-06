using UnityEngine;

//public class GridXY<GridObject>
public class GridXY
{
    private int width;
    private int height;
    private float cellSize = 1f;
    private int cellHeight;
    private Vector3 originPosition;

    private int[,] gridArray; // Terrain Height
    public Cell[,,] cellArray; // Water simulation
    private PlacedObjectData placedObjectData;

    //public GridXY(int width, int height, float cellSize, Vector3 originPosition, Func<GridXY<GridObject>, int, int, GridObject> createGridObject)
    public GridXY(int width, int height, float cellSize, int cellHeight)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.cellHeight = cellHeight;

        //gridArray = new GridObject[width, height];
        gridArray = new int[width, height];
        cellArray = new Cell[width, height, cellHeight];
        placedObjectData = new();

        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                //gridArray[x, y] = createGridObject(this, x, y);
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
    public int GetCellHeight() => cellHeight;
    //public Vector3 GetWorldPosition(int x, int y) { return new Vector3(x, y, 0) * cellSize + originPosition; }
    public void GetXY(Vector3 worldPosition, out int x, out int y)
    {
        x = Mathf.FloorToInt((worldPosition - originPosition).x / cellSize);
        y = Mathf.FloorToInt((worldPosition - originPosition).y / cellSize);
    }
    public void SetGridArray(int x, int y, int value)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            gridArray[x, y] = value;
        }
    }

    public int GetGridArray(int x, int y) => gridArray[x, y];

    public void SetCellArray(int x, int y, int z, CellType type)
    {
        cellArray[x, y, z] = new Cell(x, y, z);
        cellArray[x, y, z].Type = type;
    }

    public int GetCellArray(int x, int y)
    {
        for (int z = cellHeight - 1; z >= 0; z--)
        {
            if (cellArray[x, y, z].Liquid >= 0.01f)
                return z;
        }

        return 0;
    }

    public ref int[,] GetGridArrayRef() => ref gridArray;

    public ref Cell[,,] GetCellArrayRef() => ref cellArray;

    public ref PlacedObjectData GetPlacedObjectDataRef() => ref placedObjectData;

    public void UpdateNeighbors()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                for (int z = 0; z < cellHeight; z++)
                {
                    if (x > 0) cellArray[x, y, z].Left = cellArray[x - 1, y, z];
                    if (x < width - 2) cellArray[x, y, z].Right = cellArray[x + 1, y, z];
                    if (y > 0) cellArray[x, y, z].Down = cellArray[x, y - 1, z];
                    if (y < height - 2) cellArray[x, y, z].Up = cellArray[x, y + 1, z];
                    if (z > 0) cellArray[x, y, z].Bottom = cellArray[x, y, z - 1];
                    if (z < cellHeight - 2) cellArray[x, y, z].Top = cellArray[x, y, z + 1];
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