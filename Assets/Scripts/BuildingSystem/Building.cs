using UnityEngine;

public class Building : MonoBehaviour
{
    [SerializeField] public ObjectData data { get; set; }
    public Vector3Int GridPosition { get; set; }
    public int PlacedObjectIndex { get; set; }

    public void OnClick()
    {
        PlacementSystem.Instance.StartSelect(gameObject);
    }
}