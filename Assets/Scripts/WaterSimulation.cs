using UnityEngine;

public class WaterSimulation : MonoBehaviour
{
    private float MaxValue = 1.0f;
    private float MinValue = 0.005f;

    private float MaxCompression = 0.25f;

    private float MaxFlow = 4.0f;
    private float MinFlow = 0.005f;

    private float flowSpeed = 1f;

    private float[,] diffs;

    public void Init(Cell[,] cells)
    {
        diffs = new float[cells.GetLength(0), cells.GetLength(1)];
    }

    private float CalculateVerticalFlowValue(float remainWater, Cell destination)
    {
        float sum = remainWater + destination.liquid;
        float value = 0;

        if (sum <= MaxValue)
        {
            value = MaxValue;
        }
        else if (sum < 2 * MaxValue + MaxCompression)
        {
            value = (MaxValue * MaxValue + sum * MaxCompression) / (MaxValue + MaxCompression);
        }
        else
        {
            value = (sum + MaxCompression) / 2f;
        }

        return value;
    }

    public void Simulate(ref Cell[,] cells)
    {
        float flow = 0f;

        for (int x = 0; x < cells.GetLength(0); x++)
        {
            for (int y = 0; y < cells.GetLength(1); y++)
            {
                diffs[x, y] = 0f;
            }
        }

        for (int x = 0; x < cells.GetLength(0); x++)
        {
            for (int y = 0; y < cells.GetLength(1); y++)
            {
                Cell cell = cells[x, y];
                cell.ResetFlowDirection();

                if (cell.liquid == 0) continue;
                if (cell.settled) continue;
                if (cell.liquid < MinValue) continue;

                float startValue = cell.liquid;
                float remainValue = cell.liquid;
                flow = 0f;

                if (cell.up != null && cell.up.liquid < startValue)
                {
                    float value = CalculateVerticalFlowValue(remainValue, cell.up);
                    flow = value - cell.up.liquid;
                    if (flow > 0)
                    {
                        cell.up.flowDirections[(int)FlowDirection.Down] = true;
                        cell.flowDirections[(int)FlowDirection.Up] = true;
                        remainValue -= flow;
                        diffs[x, y] -= flow;
                        diffs[x, y + 1] += flow;
                    }
                }
            }
        }




        for(int x = 0; x < cells.GetLength(0); x++)
        {
            for(int y = 0; y < cells.GetLength(1); y++)
            {
                cells[x, y].liquid += diffs[x, y];

                if (cells[x,y].liquid < MinValue)
                {
                    cells[x, y].liquid = 0;
                    cells[x, y].settled = false; //default empty cell
                }
            }
        }
    }
}