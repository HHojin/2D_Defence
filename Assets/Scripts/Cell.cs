using UnityEngine;
using UnityEngine.Tilemaps;

public enum CellType
{
    Blank,
    Solid
}
public enum FlowDirection
{
    Up = 0,
    Right = 1,
    Down = 2,
    Left = 3,
    Top = 4,
    Bottom = 5
}

public class Cell
{
    public int X { get; private set; }
    public int Y { get; private set; }
    public int Z { get; private set; }

    public float Liquid { get; set; }

    public bool settled;
    public int settleCount;

    public CellType Type { get; set; }

    // Neighboring Cell
    public Cell Up { get; set; }
    public Cell Right { get; set; }
    public Cell Down { get; set; }
    public Cell Left { get; set; }
    public Cell Top { get; set; }
    public Cell Bottom { get; set; }

    public bool[] flowDirections = new bool[6];

    public Tilemap tilemap;
    public TileBase tilebase;

    public Cell(int x, int y, int z)
    {
        X = x;
        Y = y;
        Z = z;
    }

    public void AddWater(float amount)
    {
        Liquid += amount;
        settled = false;
    }

    public void ResetFlowDirection()
    {
        flowDirections[0] = false;
        flowDirections[1] = false;
        flowDirections[2] = false;
        flowDirections[3] = false;
        flowDirections[4] = false;
        flowDirections[5] = false;
    }

    public void UnsettledNeighbors()
    {
        if (Up != null) Up.settled = false;
        if (Right != null) Right.settled = false;
        if (Down != null) Down.settled = false;
        if (Left != null) Left.settled = false;
        if (Top != null) Top.settled = false;
        if (Bottom != null) Bottom.settled = false;
    }
}