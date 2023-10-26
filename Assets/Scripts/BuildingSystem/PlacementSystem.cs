using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlacementSystem : Singleton<PlacementSystem>
{
    private PlacedObjectData placedObjectData;

    private Vector3 lastDetectedPosition = Vector3.zero;

    private IBuildingState buildingState;

    [SerializeField] private Grid grid;

    [Header("Script")]
    [SerializeField] private InputManager inputManager;
    [SerializeField] private ObjectsDatabaseSO database;
    [SerializeField] private PreviewSystem preview;
    [SerializeField] private ObjectPlaceManager objectPlaceManager;
    [SerializeField] private InGameUI inGameUI;

    private GameObject selectedObject;

    protected override void Awake()
    {
        StopPlacement();

        placedObjectData = new();
        selectedObject = null;
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
    
    //GameScene -> click on building
    public void StartSelect(GameObject placement)
    {
        StopPlacement();
        selectedObject = placement;
        buildingState = new SelectState(placement,
                                        grid,
                                        preview,
                                        database,
                                        placedObjectData,
                                        objectPlaceManager,
                                        inGameUI);

        inputManager.OnClicked += PlaceStructure;
        inputManager.OnExit += StopPlacement;
    }

    //GameScene -> click on Destroy button
    public void DestroyPlacement()
    {
        if (buildingState == null)
            return;

        buildingState = new RemoveState(selectedObject,
                                        preview,
                                        placedObjectData,
                                        objectPlaceManager,
                                        inGameUI);

        buildingState.OnAction(selectedObject.GetComponent<Building>().GridPosition);

        StopPlacement();
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
        selectedObject = null;
    }

    private void Update()
    {
        if (buildingState == null)
            return;
        if (buildingState.GetStateType() == (int)state.Remove ||
            buildingState.GetStateType() == (int)state.SelectCantMove)
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