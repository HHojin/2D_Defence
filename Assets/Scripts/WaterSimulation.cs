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

        for(int x = 0; x < cells.GetLength(0); x++)
        {
            for(int y = 0;  y < cells.GetLength(1); y++)
            {
                diffs[x, y] = 0f;
            }
        }
    }
}
