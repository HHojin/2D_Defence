using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellComponent : MonoBehaviour
{
    // 0 = Empty // 1 = Full
    public int cellType;

    public int xGrid;
    public int yGrid;

    public Unity.Mathematics.float2 worldPos;

    public bool settled;
    public int settleCount;

    public int upIndex;
    public int downIndex;
    public int leftIndex;
    public int rightIndex;
}