using UnityEngine;

public class WaterSimulation
{
    private float MaxValue = 1.0f;
    private float MinValue = 0.005f;

    private float MaxCompression = 0.25f;

    private float MaxFlow = 4.0f;
    private float MinFlow = 0.005f;

    private float flowSpeed = 1f;

    private float[,,] diffs;

    public void Init(Cell[,,] cells)
    {
        diffs = new float[cells.GetLength(0), cells.GetLength(1), cells.GetLength(2)];
    }

    private float CalculateVerticalFlowValue(float remainWater, Cell destination)
    {
        float sum = remainWater + destination.Liquid;
        float value;

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

    public void Simulate(ref Cell[,,] cells)
    {
        float flow = 0f;

        for (int x = 0; x < cells.GetLength(0); x++)
        {
            for (int y = 0; y < cells.GetLength(1); y++)
            {
                for (int z = 0; z < cells.GetLength(2); z++)
                    diffs[x, y, z] = 0f;
            }
        }

        for (int x = 0; x < cells.GetLength(0); x++)
        {
            for (int y = 0; y < cells.GetLength(1); y++)
            {
                for (int z = 0; z < cells.GetLength(2); z++)
                {
                    Cell cell = cells[x, y, z];
                    cell.ResetFlowDirection();

                    if (cell.Liquid == 0) continue;
                    if (cell.settled) continue;
                    if (cell.Liquid < MinValue) continue;

                    float startValue = cell.Liquid;
                    float remainValue = cell.Liquid;
                    flow = 0f;

                    //Flow to bottom cell
                    if (cell.Bottom != null && cell.Bottom.Type == CellType.Blank)
                    {
                        float value = CalculateVerticalFlowValue(remainValue, cell.Bottom) - cell.Bottom.Liquid;
                        if (cell.Bottom.Liquid > 0 && flow > MinFlow)
                            flow *= flowSpeed;

                        // Constrain flow
                        flow = Mathf.Max(flow, 0);
                        if (flow > Mathf.Min(MaxFlow, cell.Liquid))
                            flow = Mathf.Min(MaxFlow, cell.Liquid);

                        // Update temp values
                        if (flow != 0)
                        {
                            remainValue -= flow;
                            diffs[x, y, z] -= flow;
                            diffs[x, y, z - 1] += flow;
                            cell.flowDirections[(int)FlowDirection.Bottom] = true;
                            cell.Bottom.settled = false;
                        }
                    }

                    if (remainValue < MinValue)
                    {
                        diffs[x, y, z] -= remainValue;
                        continue;
                    }

                    //Flow to up cell
                    if (cell.Up != null && cell.Up.Type == CellType.Blank)
                    {
                        // Calculate flow rate
                        flow = (remainValue - cell.Up.Liquid) / 6f;
                        if (flow > MinFlow)
                            flow *= flowSpeed;

                        // constrain flow
                        flow = Mathf.Max(flow, 0);
                        if (flow > Mathf.Min(MaxFlow, remainValue))
                            flow = Mathf.Min(MaxFlow, remainValue);

                        // Adjust temp values
                        if (flow != 0)
                        {
                            remainValue -= flow;
                            diffs[x, y, z] -= flow;
                            diffs[x, y + 1, z] += flow;
                            cell.flowDirections[(int)FlowDirection.Up] = true;
                            cell.Up.settled = false;
                        }
                    }

                    // Check to ensure we still have liquid in this cell
                    if (remainValue < MinValue)
                    {
                        diffs[x, y, z] -= remainValue;
                        continue;
                    }

                    // Flow to right cell
                    if (cell.Right != null && cell.Right.Type == CellType.Blank)
                    {
                        // calc flow rate
                        flow = (remainValue - cell.Right.Liquid) / 5f;
                        if (flow > MinFlow)
                            flow *= flowSpeed;

                        // constrain flow
                        flow = Mathf.Max(flow, 0);
                        if (flow > Mathf.Min(MaxFlow, remainValue))
                            flow = Mathf.Min(MaxFlow, remainValue);

                        // Adjust temp values
                        if (flow != 0)
                        {
                            remainValue -= flow;
                            diffs[x, y, z] -= flow;
                            diffs[x + 1, y, z] += flow;
                            cell.flowDirections[(int)FlowDirection.Right] = true;
                            cell.Right.settled = false;
                        }
                    }

                    // Check to ensure we still have liquid in this cell
                    if (remainValue < MinValue)
                    {
                        diffs[x, y, z] -= remainValue;
                        continue;
                    }

                    //Flow to down cell
                    if (cell.Down != null && cell.Down.Type == CellType.Blank)
                    {
                        // Calculate flow rate
                        flow = (remainValue - cell.Down.Liquid) / 4f;
                        if (flow > MinFlow)
                            flow *= flowSpeed;

                        // constrain flow
                        flow = Mathf.Max(flow, 0);
                        if (flow > Mathf.Min(MaxFlow, remainValue))
                            flow = Mathf.Min(MaxFlow, remainValue);

                        // Adjust temp values
                        if (flow != 0)
                        {
                            remainValue -= flow;
                            diffs[x, y, z] -= flow;
                            diffs[x, y - 1, z] += flow;
                            cell.flowDirections[(int)FlowDirection.Down] = true;
                            cell.Down.settled = false;
                        }
                    }

                    // Check to ensure we still have liquid in this cell
                    if (remainValue < MinValue)
                    {
                        diffs[x, y, z] -= remainValue;
                        continue;
                    }

                    //Flow to left cell
                    if (cell.Left != null && cell.Left.Type == CellType.Blank)
                    {
                        // Calculate flow rate
                        flow = (remainValue - cell.Left.Liquid) / 3f;
                        if (flow > MinFlow)
                            flow *= flowSpeed;

                        // constrain flow
                        flow = Mathf.Max(flow, 0);
                        if (flow > Mathf.Min(MaxFlow, remainValue))
                            flow = Mathf.Min(MaxFlow, remainValue);

                        // Adjust temp values
                        if (flow != 0)
                        {
                            remainValue -= flow;
                            diffs[x, y, z] -= flow;
                            diffs[x - 1, y, z] += flow;
                            cell.flowDirections[(int)FlowDirection.Left] = true;
                            cell.Left.settled = false;
                        }
                    }

                    // Check to ensure we still have liquid in this cell
                    if (remainValue < MinValue)
                    {
                        diffs[x, y, z] -= remainValue;
                        continue;
                    }

                    //Flow to top cell
                    if (cell.Top != null && cell.Top.Type == CellType.Blank)
                    {
                        //Calculate flow rate
                        flow = remainValue - CalculateVerticalFlowValue(remainValue, cell.Top);
                        if (flow > MinFlow)
                            flow *= flowSpeed;

                        // constrain flow
                        flow = Mathf.Max(flow, 0);
                        if (flow > Mathf.Min(MaxFlow, remainValue))
                            flow = Mathf.Min(MaxFlow, remainValue);

                        // Adjust temp values
                        if (flow != 0)
                        {
                            remainValue -= flow;
                            diffs[x, y, z] -= flow;
                            diffs[x, y, z + 1] += flow;
                            cell.flowDirections[(int)FlowDirection.Top] = true;
                            cell.Top.settled = false;
                        }
                    }

                    // Check to ensure we still have liquid in this cell
                    if (remainValue < MinValue)
                    {
                        diffs[x, y, z] -= remainValue;
                        continue;
                    }

                    // cell settleCount++ ...
                    if (startValue == remainValue)
                    {
                        cell.settleCount++;
                        if (cell.settleCount >= 10)
                        {
                            cell.ResetFlowDirection();
                            cell.settled = true;
                        }
                    }
                    else
                    {
                        cell.UnsettledNeighbors();
                    }
                }
            }
        }

        for (int x = 0; x < cells.GetLength(0); x++)
        {
            for (int y = 0; y < cells.GetLength(1); y++)
            {
                for (int z = 0; z < cells.GetLength(2); z++)
                {
                    cells[x, y, z].Liquid += diffs[x, y, z];

                    /*
                    if (cells[x, y, z].Liquid < MinValue)
                    {
                        cells[x, y, z].Liquid = 0;
                        cells[x, y, z].settled = false; //default empty cell
                    }
                    */
                }
            }
        }
    }
}