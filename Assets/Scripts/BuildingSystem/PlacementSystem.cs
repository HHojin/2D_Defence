using System.Collections.Generic;
using UnityEngine;

public class PlacementSystem : MonoBehaviour
{
    [SerializeField] private GameObject mouseIndicator;
    [SerializeField] private Grid grid;
    [SerializeField] private MouseController mouseController;

    [SerializeField] private ObjectsDatabaseSO database;
    private int selectedObjectID = -1;

    private PlacedObjectData placedObjectData, placedGridData;
    private List<GameObject> placedGameObjets = new();

    [SerializeField] private PreviewSystem preview;

    private Vector3 lastDetectedPosition = Vector3.zero;

    private void Start()
    {
        StopPlacement();

        placedObjectData = new();
        placedGridData = new();
    }

    public void StartPlaceMent(int ID)
    {
        StopPlacement();

        selectedObjectID = database.objectsData.FindIndex(data => data.ID == ID);
        if(selectedObjectID < 0)
        {
            Debug.LogError($"No ID found {ID}");
            return;
        }

        preview.StartShowPlcaementPreview(database.objectsData[selectedObjectID].Prefab,
                                          database.objectsData[selectedObjectID].Size);
        mouseController.OnClicked += PlaceStructure;
        mouseController.OnExit += StopPlacement;
    }

    private void PlaceStructure()
    {
        if(mouseController.IsPointerOverUI())
        {
            return;
        }

        Vector3 mousePosition = mouseController.GetMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);

        bool placementValidity = CheckPlacementValidity(gridPosition, selectedObjectID);
        if (placementValidity == false)
            return;

        GameObject newObject = Instantiate(database.objectsData[selectedObjectID].Prefab);
        newObject.transform.position = new Vector3
            (grid.CellToWorld(gridPosition).x + 0.5f,
            grid.CellToWorld(gridPosition).y + 0.5f);

        placedGameObjets.Add(newObject);
        PlacedObjectData selectedData = database.objectsData[selectedObjectID].ID == 0 ?
            placedGridData :
            placedObjectData;
        selectedData.AddObject(gridPosition,
            database.objectsData[selectedObjectID].Size,
            database.objectsData[selectedObjectID].ID,
            placedGameObjets.Count - 1);

        preview.UpdatePosition(grid.WorldToCell(gridPosition) + new Vector3(0.5f, 0.5f), false);
    }

    private bool CheckPlacementValidity(Vector3Int gridPosition, int selectedObjectID)
    {
        PlacedObjectData selectedData = database.objectsData[selectedObjectID].ID == 0 ?
            placedGridData : placedObjectData;


        return selectedData.CanPlaceObjectAt(gridPosition, database.objectsData[selectedObjectID].Size);
    }

    private void StopPlacement()
    {
        selectedObjectID = -1;
        preview.StopShowPreview();
        mouseController.OnClicked -= PlaceStructure;
        mouseController.OnExit -= StopPlacement;
        lastDetectedPosition = Vector3Int.zero;
    }

    private void Update()
    {
        if(selectedObjectID < 0)
            return;

        Vector3 mousePosition = mouseController.GetMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);
        Vector3 lastGridPosition = grid.CellToWorld(gridPosition) + new Vector3(0.5f, 0.5f);

        if(lastDetectedPosition != lastGridPosition)
        {
            bool placementValidity = CheckPlacementValidity(gridPosition, selectedObjectID);

            mouseIndicator.transform.position = mousePosition;
            preview.UpdatePosition(grid.CellToWorld(gridPosition) + new Vector3(0.5f, 0.5f), placementValidity);
            lastDetectedPosition = lastGridPosition;
        }
    }
}
