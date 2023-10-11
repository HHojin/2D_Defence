using System;
using System.Collections;
using UnityEngine;

public class WaterGenerator : MonoBehaviour
{
    private WaterSimulation waterSimulation;

    private void OnEnable()
    {
        waterSimulation = new WaterSimulation();
        waterSimulation.Init(TileManager.Instance.grid.GetCellArrayRef());
        
        StartCoroutine(AddWater());
    }


    private void Update()
    {
        //TileManager.Instance.grid.cellArray[0, 0, TileManager.Instance.grid.GetGridArray(0, 0)].AddWater(1f);
        waterSimulation.Simulate(ref TileManager.Instance.grid.GetCellArrayRef());

        for(int x = 0; x < TileManager.Instance.grid.GetWidth(); x++)
        {
            for (int y = 0; y < TileManager.Instance.grid.GetHeight(); y++)
            {
                double sum = 0;

                for (int z = 0; z < TileManager.Instance.grid.GetCellHeight(); z++)
                {
                    sum += TileManager.Instance.grid.cellArray[x, y, z].Liquid;
                }

                int tmp = (int)Math.Truncate(sum);

                if (tmp > 0.005f)
                {
                    TileManager.Instance.DrawTile(x, y, 1, tmp);
                }
                else
                {
                    TileManager.Instance.DrawTile(x, y, 1, -1);
                }
            }
        }
    }

    IEnumerator AddWater()
    {
        while (true)
        {
            TileManager.Instance.grid.cellArray[0, 0, TileManager.Instance.grid.GetGridArray(0, 0)].AddWater(0.5f);
            yield return new WaitForSeconds(0.05f);
        }
    }

    /*
    public void StopCoroutine()
    {
        StopAllCoroutines();
    }


    public void StartSimulation()
    {
        StartCoroutine(AddWater());
        StartCoroutine(Simulation());
        StartCoroutine(DrawWater());
    }


    IEnumerator Simulation()
    {
        while (true)
        {
            waterSimulation.Simulate(ref TileManager.Instance.grid.cellArray);
            yield return new WaitForEndOfFrame();
        }
    }


    IEnumerator DrawWater()
    {
        while (true)
        {
            foreach(var cell in TileManager.Instance.grid.cellArray)
            {
                if (cell.Liquid > 0f)
                {
                    map.SetTile(new Vector3Int(cell.X, cell.Y, 0), TileManager.Instance.waterTileBase);
                }
                else
                {
                    map.SetTile(new Vector3Int(cell.X, cell.Y, 0), null);
                }
            }
        }
    }
    */
}
