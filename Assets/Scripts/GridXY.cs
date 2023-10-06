using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public class GridXY<GridObject>
public class GridXY
{
    public event EventHandler<OnGridObjectChangedEventArgs> OnGridObjectChanged;

    public class OnGridObjectChangedEventArgs : EventArgs
    {
        public int x;
        public int y;
    }

    public int width;
    public int height;
    private float cellSize = 1f;
    private Vector3 originPosition;
    //private GridObject[,] gridArray;
    private int[,] gridArray; // Terrain Height
    private Cell[,] cells; // water simulation

    //public GridXY(int width, int height, float cellSize, Vector3 originPosition, Func<GridXY<GridObject>, int, int, GridObject> createGridObject)
    //public GridXY(int width, int height, float cellSize, GameObject tile)
    public GridXY(int width, int height, float cellSize)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;

        //gridArray = new GridObject[width, height];
        gridArray = new int[width, height];
        cells = new Cell[width, height];

        for(int x = 0; x < gridArray.GetLength(0); x++)
        {
            for(int y = 0; y < gridArray.GetLength(1); y++)
            {
                //gridArray[x, y] = createGridObject(this, x, y);
                //Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white, 10f);
                //Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white, 10f);
            }
        }

        //Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, 10f);
        //Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, 10f);
    }

    public int GetWidth() { return width; }
    public int GetHeight() { return height; }
    public float GetCellSize() {  return cellSize; }
    public Vector3 GetWorldPosition(int x, int y) {  return new Vector3(x, y, 0) * cellSize + originPosition; }
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

    public int GetGridArray(int x, int y) { return gridArray[x, y]; }

    public void UpdateNeighbors()
    {
        for(int x = 0; x < width; x++)
        {
            for(int y = 0; y < height; y++)
            {
                if(x > 0) cells[x, y].left = cells[x - 1, y];
                if(x < width - 1) cells[x, y].right = cells[x + 1, y];
                if(y > 0) cells[x, y].down = cells[x, y - 1];
                if(y < height - 1) cells[x, y].up = cells[x, y + 1];
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