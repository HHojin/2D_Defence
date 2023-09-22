using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class MapGenerator : MonoBehaviour
{
    [SerializeField]
    private int gridWidth;
    [SerializeField]
    private int gridHeight;

    List<List<int>> noiseGrid;
    List<List<GridXY<GridObject>>> gridList;

    public float magnification = 7.0f; // 4 ~ 20

    float xOffset = 0f; // <- +>
    float yOffset = 0f;

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

    private TileManager tileManager;
    private CameraManager cameraManager;

    private void Awake()
    {
        gridWidth = 100;
        gridHeight = 100;

        tileManager = gameObject.GetComponent<TileManager>();
        cameraManager = FindObjectOfType<CameraManager>();
    }

    private void Start()
    {
        //GenerateMap();
    }

    public void GenerateMap()
    {
        noiseGrid = new List<List<int>>();
        gridList = new List<List<GridXY<GridObject>>>();
        tileManager.ResetTileMap();

        magnification = sliderMag.value;
        xOffset = Random.Range(0f, 99999f);
        yOffset = Random.Range(0f, 99999f);

        for (int x = 0; x < gridWidth; x++)
        {
            noiseGrid.Add(new List<int>());
            gridList.Add(new List<GridXY<GridObject>>());
            for (int y = 0; y < gridHeight; y++)
            {
                int tileId = GetIdUsingPerlin(x, y);

                noiseGrid[x].Add(tileId);
                tileManager.DrawTile(x, y, tileId);
            }
        }

        tileManager.SetPolygonCollider(gridWidth / 2, gridHeight / 2);
        cameraManager.SetCameraPosition(gridWidth / 4, gridHeight / 4);
    }

    int GetIdUsingPerlin(int x, int y)
    {
        float rawPerlin = Mathf.PerlinNoise(
            (x - xOffset) / magnification,
            (y - yOffset) / magnification
        );

        float clampPerlin = Mathf.Clamp(rawPerlin, 0.0f, 1.0f);
        float scalePerlin = clampPerlin * tileManager.GetTileBaseCount();

        if (scalePerlin >= 9) scalePerlin = 9;

        return Mathf.FloorToInt(scalePerlin); // 0 ~ 9
    }
}
