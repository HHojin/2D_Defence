using UnityEngine;

public class Building : MonoBehaviour
{
    [SerializeField] public ObjectData data { get; set; }

    public void OnClick()
    {
        PlacementSystem.Instance.StartSelect(gameObject);
    }
}