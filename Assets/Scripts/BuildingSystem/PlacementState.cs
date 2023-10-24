using UnityEngine;

public class PlacementState : IBuildingState
{
    private int selectedObjectID = -1;
    private int ID;
    private Grid grid;
    private PreviewSystem previewSystem;
    private ObjectsDatabaseSO database;
    private PlacedObjectData placedObjectData;
    private ObjectPlaceManager objectPlaceManager;

    public PlacementState(int iD,
                          Grid grid,
                          PreviewSystem previewSystem,
                          ObjectsDatabaseSO database,
                          PlacedObjectData placedObjectData,
                          ObjectPlaceManager objectPlaceManager)
    {
        ID = iD;
        this.grid = grid;
        this.previewSystem = previewSystem;
        this.database = database;
        this.placedObjectData = placedObjectData;
        this.objectPlaceManager = objectPlaceManager;

        selectedObjectID = database.objectsData.FindIndex(data => data.ID == ID);
        if (selectedObjectID > -1)
        {
            previewSystem.StartShowPlcaementPreview(database.objectsData[selectedObjectID].Prefab,
                                                    database.objectsData[selectedObjectID].Size);
        }
        else
            throw new System.Exception($"No ID found {iD}");
    }

    public int GetStateType()
    {
        return (int)state.Placement;
    }

    public void OnAction(Vector3Int gridPosition)
    {
        bool placementValidity = CheckPlacementValidity(gridPosition, selectedObjectID);
        if (placementValidity == false)
            return;

        int index = objectPlaceManager.PlaceObject(database.objectsData[selectedObjectID],
                                                   gridPosition,
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
    }
}