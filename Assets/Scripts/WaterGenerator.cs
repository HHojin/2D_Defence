using System.Collections;
using UnityEngine;

using UnityEngine.Events;

public class WaterGenerator : MonoBehaviour
{
    private WaterSimulation waterSimulation;

    private int length;
    private int width;
    private int height;

    [HideInInspector] private UnityEvent<float, float> mapGenerated;

    private void OnEnable()
    {
        waterSimulation = new WaterSimulation();
        waterSimulation.Init(TileManager.Instance.grid.GetCellArrayRef());

        length = TileManager.Instance.grid.GetWidth();
        width = TileManager.Instance.grid.GetHeight();
        height = TileManager.Instance.grid.GetCellHeight();
        Debug.Log($"{length}, {width}, {height}");

        GameManager.Instance.mapGenerated.AddListener(StopCoroutine);

        StartCoroutine(Simulation());
        StartCoroutine(AddWater());
        StartCoroutine(DrawTile());
    }

    public void StopCoroutine(float x, float y)
    {
        StopAllCoroutines();

        TileManager.Instance.ResetTileMap(1);

        gameObject.GetComponent<WaterGenerator>().enabled = false;
    }

    IEnumerator Simulation()
    {
        while (true)
        {
            waterSimulation.Simulate(ref TileManager.Instance.grid.GetCellArrayRef());
            yield return YieldInStructionCache.WaitForSeconds(5.0f);
        }
    }

    IEnumerator DrawTile()
    {
        int waterHeight;
        while (true)
        {
            for (int x = 0; x < length; x++)
            {
                for (int y = 0; y < width; y++)
                {
                    waterHeight = -1;

                    for (int z = height - 1; z >= 0; z--)
                    {
                        if (TileManager.Instance.grid.cellArray[x, y, z].Liquid > 0.01f)
                        {
                            waterHeight = z;
                            break;
                        }
                    }

                    TileManager.Instance.DrawTile(x, y, 1, waterHeight);
                }
            }

            yield return YieldInStructionCache.WaitForSeconds(1.0f);
        }
    }

    IEnumerator AddWater()
    {
        while (true)
        {
            TileManager.Instance.grid.cellArray[0, 0, TileManager.Instance.grid.GetGridArray(0, 0)].AddWater(0.1f);
            yield return YieldInStructionCache.WaitForSeconds(10.0f);
        }
    }

    /*
    private void Update()
    {
        //waterSimulation.Simulate(ref TileManager.Instance.grid.GetCellArrayRef());
        //DrawTile();
    }

    private void DrawTile()
    {
        int waterHeight;

        for (int x = 0; x < length; x++)
        {
            for (int y = 0; y < width; y++)
            {
                waterHeight = -1;

                for (int z = height - 1; z >= 0; z--)
                {
                    if (TileManager.Instance.grid.cellArray[x, y, z].Liquid > 0.005f)
                    {
                        waterHeight = z;
                        break;
                    }
                }

                TileManager.Instance.DrawTile(x, y, 1, waterHeight);
            }
        }
    }
    */
}