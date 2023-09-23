using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class MouseController : MonoBehaviour
{
    [Header ("Sreen Controll")]
    [SerializeField] private float panSpeed = 15f;
    [SerializeField] private float zoomScale = 2.5f;

    [Space(10f)]
    [SerializeField] private float zoomInMax = 10f;
    [SerializeField] private float zoomOutMax = 20f;

    private CinemachineInputProvider inputProvider;
    private CinemachineVirtualCamera virtualCamera;
    private Transform cameraTransform;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Confined;

        inputProvider = GetComponent<CinemachineInputProvider>();
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        cameraTransform = virtualCamera.VirtualCameraGameObject.transform;
    }

    private void Update()
    {
        float x = inputProvider.GetAxisValue(0);
        float y = inputProvider.GetAxisValue(1);
        float z = inputProvider.GetAxisValue(2);

        if (x != 0 || y != 0)
        {
            PanScreen(x, y);
        }
        if (z != 0)
        {
            ZoomScreen(z);
        }
    }

    public Vector2 PanDirection(float x, float y)
    {
        Vector2 direction = Vector2.zero;

        if (y >= Screen.height * 0.98f)
        {
            direction.y += 1;
        }
        if (x >= Screen.width * 0.98f)
        {
            direction.x += 1;
        }
        if (y <= Screen.height * 0.02f)
        {
            direction.y -= 1;
        }
        if (x <= Screen.width * 0.02f)
        {
            direction.x -= 1;
        }
        return direction;
    }

    public void PanScreen(float x, float y)
    {
        Vector2 direction = PanDirection(x, y);
        cameraTransform.position = Vector3.Lerp(cameraTransform.position,
                                                cameraTransform.position + (Vector3)direction * panSpeed,
                                                Time.deltaTime);
    }

    public void ZoomScreen(float increment)
    {
        float fov = virtualCamera.m_Lens.OrthographicSize;
        float target = (increment > 0) ? fov + zoomScale : fov - zoomScale;

        virtualCamera.m_Lens.OrthographicSize = Mathf.Clamp(target, zoomInMax, zoomOutMax);
    }
}
