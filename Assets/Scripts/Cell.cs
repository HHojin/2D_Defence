public enum CellType
{
    Blank,
    Wall
}
public enum FlowDirection
{
    Up = 0,
    Right = 1,
    Down = 2,
    Left = 3,
}

public class Cell
{
    public int xGrid { get; private set; }
    public int yGrid { get; private set; }

    public float liquid { get; set; }

    public bool settled;
    public int settleCount;

    public CellType type { get; private set; }

    // Neighboring Cell
    public Cell up { get; set; }
    public Cell right { get; set; }
    public Cell down { get; set; }
    public Cell left { get; set; }

    public bool[] flowDirections = new bool[4];

    public Cell(int x, int y)
    {
        xGrid = x;
        yGrid = y;
    }

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

    public void UnsettledNeigthbors()
    {
        if (up != null) up.settled = false;
        if (right != null) right.settled = false;
        if (down != null) down.settled = false;
        if (left != null) left.settled = false;
    }
}