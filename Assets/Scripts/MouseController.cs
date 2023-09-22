using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : MonoBehaviour
{
    private float zoomSpeed = 10.0f;
    public Camera mainCamera;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Confined;
    }

    private void Update()
    {
        MousePos();
        Zoom();
    }

    private void MousePos()
    {
        var mouseWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0;
        this.gameObject.transform.position = mouseWorldPos;
    }

    private void Zoom()
    {
        float distance = Input.GetAxis("Mouse ScrollWheel") * -1 * zoomSpeed;
        if (distance != 0)
        {

        }
    }

    public void SetMousePosition(float x, float y)
    {
        this.gameObject.transform.position = new Vector3(x, y, 0);
    }
}
