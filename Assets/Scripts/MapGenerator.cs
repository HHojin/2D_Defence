using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapGenerator : MonoBehaviour
{
    [SerializeField]
    private int gridWidth;
    [SerializeField]
    private int gridHeight;

    List<List<int>> noiseGrid;
    List<List<GridXY<GridObject>>> gridList;

    public float magnification = 7.0f; // 4 ~ 20

    int xOffset = 0; // <- +>
    int yOffset = 0;

    // test slider
    public Slider sliderMag;
    public Slider sliderXOffset;
    public Slider sliderYOffset;

    public class GridObject
    {
        private GridXY<GridObject> grid;
        private int x;
        private int y;
        public PlacedObject placedObject;

        public GridObject(GridXY<GridObject> grid, int x, int y)
        {
            this.grid = grid;
            this.x = x;
            this.y = y;
            placedObject = null;
        }

        public override string ToString()
        {
            return x + ", " + y + "\n" + placedObject;
        }

        public void TriggerGridObjectChanged()
        {
            grid.TriggerGridObjectChanged(x, y);
        }

        public void SetPlacedObject(PlacedObject placedObject)
        {
            this.placedObject = placedObject;
            TriggerGridObjectChanged();
        }

        public void ClearPlacedObject()
        {
            placedObject = null;
            TriggerGridObjectChanged();
        }

        public PlacedObject GetPlacedObject()
        {
            return placedObject;
        }

        public bool CanBuild()
        {
            return placedObject == null;
        }
    }

    private TileController tileController;

    private void Awake()
    {
        gridWidth = 100;
        gridHeight = 100;

        tileController = gameObject.GetComponent<TileController>();

    }

    private void Start()
    {
        //GenerateMap();
    }

    public void GenerateMap()
    {
        noiseGrid = new List<List<int>>();
        gridList = new List<List<GridXY<GridObject>>>();
        tileController.ResetTileMap();

        //test
        magnification = sliderMag.value;
        xOffset = (int)sliderXOffset.value;
        yOffset = (int)sliderYOffset.value;

        for (int x = 0; x < gridWidth; x++)
        {
            noiseGrid.Add(new List<int>());
            gridList.Add(new List<GridXY<GridObject>>());
            for (int y = 0; y < gridHeight; y++)
            {
                int tileId = GetIdUsingPerlin(x, y);

                noiseGrid[x].Add(tileId);
                //CreateTile(tileId, x, y);
                tileController.DrawTile(x, y, tileId);
            }
        }
    }

    int GetIdUsingPerlin(int x, int y)
    {
        float rawPerlin = Mathf.PerlinNoise(
            (x - xOffset) / magnification,
            (y - yOffset) / magnification
        );

        float clampPerlin = Mathf.Clamp(rawPerlin, 0.0f, 1.0f);
        float scalePerlin = clampPerlin * tileController.GetTileBaseCount();

        if (scalePerlin >= 9) scalePerlin = 9;

        return Mathf.FloorToInt(scalePerlin); // 0 ~ 9
    }
}