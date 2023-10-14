using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    private int order = 10;

    private Stack<UIPopUp> popupStack = new Stack<UIPopUp>();
    private UIScene sceneUI = null;

    public GameObject Root
    {
        get
        {
            GameObject root = GameObject.Find("UI");
            if (root == null)
            {
                root = new GameObject(name = "@UIRoot");
            }
            return root;
        }
    }

    public void SetCanvas(GameObject go, bool sort = true)
    {
        Canvas canvas = Utils.GetOrAddComponent<Canvas>(go);
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.overrideSorting = true;

        if (sort)
        {
            canvas.sortingOrder = order;
            order++;
        }
        else
        {
            canvas.sortingOrder = 0;
        }
    }

    public T ShowSceneUI<T>(string name = null) where T : UIScene
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        GameObject go = null;//Managers.Resource.Instantiate($"UI/Scene/{name}");
        T sceneUI = Utils.GetOrAddComponent<T>(go);
        this.sceneUI = sceneUI;

        go.transform.SetParent(Root.transform);

        return sceneUI;
    }

    public T ShowPopupUI<T>(string name = null) where T : UIPopUp
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        GameObject go = null;//Managers.Resource.Instantiate($"UI/Popup/{name}");
        T popup = Utils.GetOrAddComponent<T>(go);
        popupStack.Push(popup);

        go.transform.SetParent(Root.transform);

        return popup;
    }

    public void ClosePopupUI(UIPopUp popup) // 안전 차원
    {
        if (popupStack.Count == 0)
            return;

        if (popupStack.Peek() != popup)
        {
            Debug.Log("Close Popup Failed!");
            return;
        }

        ClosePopupUI();
    }

    public void ClosePopupUI()
    {
        if (popupStack.Count == 0)
            return;

        UIPopUp popup = popupStack.Pop();
        //Managers.Resource.Destroy(popup.gameObject);
        popup = null;
        order--; // order 줄이기
    }

    public void CloseAllPopupUI()
    {
        while (popupStack.Count > 0)
            ClosePopupUI();
    }
}