using UnityEngine;

public class PlacementSystem : MonoBehaviour
{
    [SerializeField] private GameObject mouseIndicator, cellIndicator;
    [SerializeField] private Grid grid;

    private void Update()
    {
        Vector3 mousePosition = GetMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);
        mouseIndicator.transform.position = mousePosition;
        cellIndicator.transform.position = new Vector3(grid.CellToWorld(gridPosition).x + 0.5f, grid.CellToWorld(gridPosition).y + 0.5f);
    }




    private Vector2 GetMapPosition()
    {
        var mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (mouseWorldPos.x >= 0 && mouseWorldPos.y >= 0
            && mouseWorldPos.x <= TileManager.Instance.grid.GetWidth()
            && mouseWorldPos.y <= TileManager.Instance.grid.GetHeight())
        {
            //return new Vector2(mouseWorldPos.x + 0.5f, mouseWorldPos.y + 0.5f);
            return mouseWorldPos;
        }
        else
        {
            return new Vector2(0, 0);
        }
    }
}
