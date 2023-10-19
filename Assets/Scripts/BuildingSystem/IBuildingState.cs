using UnityEngine;

public interface IBuildingState
{
    enum state
    {
        Placement,
        Select,
        Remove
    }

    int GetStateType();
    void OnAction(Vector3Int gridPosition);
    void UpdateState(Vector3Int gridPosition);
    void EndState();
}