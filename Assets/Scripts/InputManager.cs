using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour
{
    /*
    public GameObject buildingUI;
    public GameObject objectUI;
    */
    public event Action OnClicked, OnExit;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 clickPos = new Vector2(worldPos.x, worldPos.y);
            Collider2D clickColl = Physics2D.OverlapPoint(clickPos);

            if (clickColl != null && clickColl.CompareTag("Building"))
            {
                clickColl.GetComponent<Building>().OnClick();
            }
            

            OnClicked?.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            /*
            buildingUI.SetActive(true);
            objectUI.SetActive(false);
            */
            OnExit?.Invoke();
        }
    }

    //BuildingSystem..
    public bool IsPointerOverUI() => EventSystem.current.IsPointerOverGameObject();

    public Vector2 GetMapPosition()
    {
        var mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (mouseWorldPos.x >= 0 && mouseWorldPos.y >= 0
            && mouseWorldPos.x < TileManager.Instance.grid.GetWidth()
            && mouseWorldPos.y < TileManager.Instance.grid.GetHeight())
        {
            //return new Vector2(mouseWorldPos.x + 0.5f, mouseWorldPos.y + 0.5f);
            return mouseWorldPos;
        }
        else
        {
            return new Vector2(-1, -1);
        }
    }
}
