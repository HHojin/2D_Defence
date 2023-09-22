using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    //private float panSpeed = 20f;
    //private float panBorderThickness = 5;
    //private Vector2 panLimit;
    private Camera mainCamera;

    private void Awake()
    {
        mainCamera = GetComponent<Camera>();
    }

    /*
    private void Update()
    {
        Vector3 pos = transform.position;
        var mouseWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetKey("w") || mouseWorldPos.y >= Screen.height - 5)
            pos.y += panSpeed * Time.deltaTime;
        if (Input.GetKey("s") || mouseWorldPos.y <= 5)
            pos.y -= panSpeed * Time.deltaTime;
        if (Input.GetKey("a") || mouseWorldPos.x >= Screen.width - 5)
            pos.x += panSpeed * Time.deltaTime;
        if (Input.GetKey("d") || mouseWorldPos.x <= 5)
            pos.x -= panSpeed * Time.deltaTime;

        pos.x = Mathf.Clamp(pos.x, -5, panLimit.x);
        pos.y = Mathf.Clamp(pos.y, -5, panLimit.y);

        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, 10, 30);

        transform.position = pos;
    }
    */

    public void SetCameraPosition(float x, float y)
    {
        this.transform.position = new Vector3(x, y, -10);
    }

}
