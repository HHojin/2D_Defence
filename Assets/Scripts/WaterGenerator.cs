using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WaterGenerator : MonoBehaviour
{
    private WaterSimulation waterSimulation;
    public Tilemap[] map = new Tilemap[5];

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

        foreach (var cell in TileManager.Instance.grid.cellArray)
        {
            if (cell.Liquid > 0.005f)
            {
                map[cell.Z].SetTile(new Vector3Int(cell.X, cell.Y, 0), TileManager.Instance.waterTileBase);
            }
            else
            {
                map[cell.Z].SetTile(new Vector3Int(cell.X, cell.Y, 0), null);
            }
        }
    }

    IEnumerator AddWater()
    {
        while (true)
        {
            TileManager.Instance.grid.cellArray[0, 0, TileManager.Instance.grid.GetGridArray(0, 0)].AddWater(1f);
            yield return new WaitForSeconds(0.1f);
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
