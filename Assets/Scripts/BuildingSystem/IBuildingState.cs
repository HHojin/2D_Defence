using UnityEngine;

public enum state
{
    Placement,
    SelectCanMove,
    SelectCantMove,
    Remove
}

public interface IBuildingState
{
    int GetStateType();
    void OnAction(Vector3Int gridPosition);
    void UpdateState(Vector3Int gridPosition);
    void EndState();
}