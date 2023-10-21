using UnityEngine;

public class RemoveState : IBuildingState
{
    private int gameObjectID = -1;
    private GameObject placement;
    private Grid grid;
    private PreviewSystem previewSystem;
    private PlacedObjectData placedObjectData;
    private ObjectPlaceManager objectPlaceManager;

    public RemoveState(GameObject placement,
                       Grid grid,
                       PreviewSystem previewSystem,
                       PlacedObjectData placedObjectData,
                       ObjectPlaceManager objectPlaceManager)
    {
        this.placement = placement;
        this.grid = grid;
        this.previewSystem = previewSystem;
        this.placedObjectData = placedObjectData;
        this.objectPlaceManager = objectPlaceManager;

        previewSystem.StartShowRemovePreview();
    }

    public int GetStateType()
    {
        return (int)state.Remove;
    }

    public void OnAction(Vector3Int gridPosition)
    {
        PlacedObjectData selectedData = placedObjectData;

        selectedData.RemoveObjectAt(gridPosition);
        objectPlaceManager.RemoveObjectAt(gameObjectID);

        Vector3 gridPlacementPosition = grid.CellToWorld(gridPosition) + new Vector3(0.5f, 0.5f);
        previewSystem.UpdatePosition(gridPlacementPosition, true);
    }

    public void UpdateState(Vector3Int gridPosition)
    {
        throw new System.NotImplementedException();
    }

    public void EndState()
    {
        previewSystem.StopShowPreview();
    }
}