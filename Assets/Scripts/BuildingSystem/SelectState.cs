using UnityEngine;

public class SelectState : IBuildingState
{
    private int selectedObjectID = -1;
    private Grid grid;
    private PreviewSystem previewSystem;
    private ObjectsDatabaseSO database;
    private PlacedObjectData placedObjectData;
    private ObjectPlaceManager objectPlaceManager;
    private InGameUI inGameUI;

    public SelectState(GameObject placement,
                       Grid grid,
                       PreviewSystem previewSystem,
                       ObjectsDatabaseSO database,
                       PlacedObjectData placedObjectData,
                       ObjectPlaceManager objectPlaceManager,
                       InGameUI inGameUI)
    {
        this.grid = grid;
        this.previewSystem = previewSystem;
        this.database = database;
        this.placedObjectData = placedObjectData;
        this.objectPlaceManager = objectPlaceManager;
        this.inGameUI = inGameUI;

        selectedObjectID = placement.GetComponent<Building>().data.ID;
        inGameUI.OnClickObject();

        if (selectedObjectID >= 8)
        {
            previewSystem.StartShowPlcaementPreview(database.objectsData[selectedObjectID].Prefab,
                                                    database.objectsData[selectedObjectID].Size);
        }
        //else
        //    throw new System.Exception($"No ID found {selectedObjectID}");
    }

    public int GetStateType()
    {
        return selectedObjectID >= 8 ? (int)state.SelectCanMove : (int)state.SelectCantMove;
    }

    public void OnAction(Vector3Int gridPosition)
    {
        if (selectedObjectID < 8)
            return;

        bool placementValidity = CheckPlacementValidity(gridPosition, selectedObjectID);
        if (placementValidity == false)
            return;

        int index = objectPlaceManager.PlaceObject(database.objectsData[selectedObjectID],
                                                   new Vector3(grid.CellToWorld(gridPosition).x + 0.5f,
                                                               grid.CellToWorld(gridPosition).y + 0.5f));

        PlacedObjectData selectedData = placedObjectData;
        selectedData.AddObject(gridPosition,
            database.objectsData[selectedObjectID].Size,
            database.objectsData[selectedObjectID].ID,
            index);

        previewSystem.UpdatePosition(grid.WorldToCell(gridPosition) + new Vector3(0.5f, 0.5f), false);
    }

    private bool CheckPlacementValidity(Vector3Int gridPosition, int selectedObjectID)
    {
        PlacedObjectData selectedData = placedObjectData;

        return selectedData.CanPlaceObjectAt(gridPosition, database.objectsData[selectedObjectID].Size);
    }

    public void UpdateState(Vector3Int gridPosition)
    {
        bool placementValidity = CheckPlacementValidity(gridPosition, selectedObjectID);

        previewSystem.UpdatePosition(grid.CellToWorld(gridPosition) + new Vector3(0.5f, 0.5f), placementValidity);
    }

    public void EndState()
    {
        previewSystem.StopShowPreview();
        inGameUI.OnExitObject();
    }
}
