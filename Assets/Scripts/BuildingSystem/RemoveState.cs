using UnityEngine;

public class RemoveState : IBuildingState
{
    private int selectedObjectIndex = -1;
    private PreviewSystem previewSystem;
    private PlacedObjectData placedObjectData;
    private ObjectPlaceManager objectPlaceManager;
    private InGameUI inGameUI;

    public RemoveState(GameObject placement,
                       PreviewSystem previewSystem,
                       PlacedObjectData placedObjectData,
                       ObjectPlaceManager objectPlaceManager,
                       InGameUI inGameUI)
    {
        this.previewSystem = previewSystem;
        this.placedObjectData = placedObjectData;
        this.objectPlaceManager = objectPlaceManager;
        this.inGameUI = inGameUI;

        selectedObjectIndex = placement.GetComponent<Building>().PlacedObjectIndex;
    }

    public int GetStateType()
    {
        return (int)state.Remove;
    }

    public void OnAction(Vector3Int gridPosition)
    {
        PlacedObjectData selectedData = placedObjectData;

        selectedData.RemoveObjectAt(gridPosition);
        objectPlaceManager.RemoveObjectAt(selectedObjectIndex);
    }

    public void UpdateState(Vector3Int gridPosition)
    {
        throw new System.NotImplementedException();
    }

    public void EndState()
    {
        previewSystem.StopShowSelectedPlacement();
        previewSystem.StopShowPreview();
        inGameUI.OnExitObject();
    }
}