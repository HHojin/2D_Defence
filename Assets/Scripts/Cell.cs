using UnityEngine;

public enum FlowDirection
{
    Up = 0,
    Right = 1,
    Down = 2,
    Left = 3,
}

public class Cell: MonoBehaviour
{
    // 0 = Empty // 1 = Full
    public int cellType;

    public int xGrid { get; private set; }
    public int yGrid { get; private set; }

    public float liquid { get; set; }

    public bool settled;
    public int settleCount;

    // Neighboring Cell
    public Cell top { get; set; }
    public Cell right { get; set; }
    public Cell down { get; set; }
    public Cell left { get; set; }

    public bool[] flowDirections = new bool[4];

    public void AddWater(float amount)
    {
        liquid += amount;
        settled = false;
    }

    public void ResetFlowDirection()
    {
        flowDirections[0] = false;
        flowDirections[1] = false;
        flowDirections[2] = false;
        flowDirections[3] = false;
    }
}