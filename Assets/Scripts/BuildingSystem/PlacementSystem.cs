using System.Collections.Generic;
using UnityEngine;

public class PlacementSystem : MonoBehaviour
{
    [SerializeField] private Grid grid;
    [SerializeField] private MouseController mouseController;

    [SerializeField] private ObjectsDatabaseSO database;

    private PlacedObjectData placedObjectData;

    [SerializeField] private PreviewSystem preview;

    private Vector3 lastDetectedPosition = Vector3.zero;

    [SerializeField] private ObjectPlaceManager objectPlaceManager;

    private IBuildingState buildingState;

    private void Start()
    {
        StopPlacement();

        placedObjectData = new();
    }

    public void StartPlaceMent(int ID)
    {
        if (!GameManager.Instance.isMapGenerated)
            return;

        StopPlacement();
        buildingState = new PlacementState(ID,
                                           grid,
                                           preview,
                                           database,
                                           placedObjectData,
                                           objectPlaceManager);

        mouseController.OnClicked += PlaceStructure;
        mouseController.OnExit += StopPlacement;
    }

    private void PlaceStructure()
    {
        if (mouseController.IsPointerOverUI())
        {
            return;
        }

        Vector3 mousePosition = mouseController.GetMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);

        buildingState.OnAction(gridPosition);
    }

    public void StopPlacement()
    {
        if (buildingState == null)
            return;

        buildingState.EndState();

        mouseController.OnClicked -= PlaceStructure;
        mouseController.OnExit -= StopPlacement;
        lastDetectedPosition = Vector3Int.zero;

        buildingState = null;
    }

    private void Update()
    {
        if (buildingState == null)
            return;

        Vector3 mousePosition = mouseController.GetMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);
        Vector3 lastGridPosition = grid.CellToWorld(gridPosition) + new Vector3(0.5f, 0.5f);

        if (lastDetectedPosition != lastGridPosition)
        {
            buildingState.UpdateState(gridPosition);

            lastDetectedPosition = lastGridPosition;
        }
    }
}
