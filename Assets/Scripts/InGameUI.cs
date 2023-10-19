using UnityEngine;
using UnityEngine.UI;

public class InGameUI : MonoBehaviour
{
    [Header("Height Bar")]
    public Image[] heightBar;
    private Color terrain = new Color(0f, 255f, 0f, 255f);
    private Color water = new Color(0f, 0f, 255f, 255f);
    private Color none = new Color(0f, 0f, 0f, 0f);

    private void Update()
    {
        if(GameManager.Instance.isMapGenerated)
            ShowHeight();
    }

    public void ShowHeight()
    {
        var mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (mouseWorldPos.x >= 0 && mouseWorldPos.y >= 0
            && mouseWorldPos.x <= TileManager.Instance.grid.GetWidth()
            && mouseWorldPos.y <= TileManager.Instance.grid.GetHeight())
        {
            int idx = 0;

            var gridPos = TileManager.Instance.map.WorldToCell(mouseWorldPos);
            var gridValue = TileManager.Instance.grid.GetGridArray(gridPos.x, gridPos.y);

            for (; idx <= gridValue; idx++)
            {
                heightBar[idx].color = terrain;
            }

            var cellPos = TileManager.Instance.water.WorldToCell(mouseWorldPos);
            var cellValue = TileManager.Instance.grid.GetCellArray(cellPos.x, cellPos.y) + 1;
            if(cellValue > 1)
                for (; idx <= cellValue; idx++)
                {
                    heightBar[idx].color = water;
                }

            for (; idx < heightBar.Length; idx++)
            {
                heightBar[idx].color = none;
            }
        }
        else
        {
            for (int i = 0; i < heightBar.Length; i++)
            {
                heightBar[i].color = none;
            }
        }
    }
}