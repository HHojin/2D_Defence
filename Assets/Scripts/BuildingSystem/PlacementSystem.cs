using UnityEngine;

public class PlacementSystem : Singleton<PlacementSystem>
{
    [SerializeField] private Grid grid;
    [SerializeField] private InputManager inputManager;

    [SerializeField] private ObjectsDatabaseSO database;

    private PlacedObjectData placedObjectData;

    [SerializeField] private PreviewSystem preview;

    private Vector3 lastDetectedPosition = Vector3.zero;

    [SerializeField] private ObjectPlaceManager objectPlaceManager;

    private IBuildingState buildingState;

    protected override void Awake()
    {
        StopPlacement();

        placedObjectData = new();
    }

    //BuildingUI -> UI button
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

        inputManager.OnClicked += PlaceStructure;
        inputManager.OnExit += StopPlacement;
    }
    
    public void StartSelect(GameObject placement)
    {
        StopPlacement();
        buildingState = new SelectState(placement,
                                        grid,
                                        preview,
                                        database,
                                        placedObjectData,
                                        objectPlaceManager);

        inputManager.OnClicked += PlaceStructure;
        inputManager.OnExit += StopPlacement;
    }
    

    public void StartRemove(GameObject placement)
    {
        StopPlacement();
        buildingState = new RemoveState(placement,
                                        grid,
                                        preview,
                                        placedObjectData,
                                        objectPlaceManager);

        inputManager.OnClicked += PlaceStructure;
        inputManager.OnExit += StopPlacement;
    }

    private void PlaceStructure()
    {
        if (inputManager.IsPointerOverUI())
        {
            return;
        }

        Vector3 mousePosition = inputManager.GetMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);

        buildingState.OnAction(gridPosition);
    }

    public void StopPlacement()
    {
        if (buildingState == null)
            return;

        buildingState.EndState();

        inputManager.OnClicked -= PlaceStructure;
        inputManager.OnExit -= StopPlacement;
        lastDetectedPosition = Vector3Int.zero;

        buildingState = null;
    }

    private void Update()
    {
        if (buildingState == null)
            return;
        if (buildingState.GetStateType() == (int)state.Remove)
            return;

        Vector3 mousePosition = inputManager.GetMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);
        Vector3 lastGridPosition = grid.CellToWorld(gridPosition) + new Vector3(0.5f, 0.5f);

        if (gridPosition.x != -1 && gridPosition.y != -1)
        {
            if (lastDetectedPosition != lastGridPosition)
            {
                preview.InsideOfMap();
                buildingState.UpdateState(gridPosition);

                lastDetectedPosition = lastGridPosition;
            }
        }
        else
        {
            preview.OutOfMap();
        }
    }
}