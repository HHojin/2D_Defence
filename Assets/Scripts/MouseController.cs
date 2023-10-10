using Cinemachine;
using UnityEngine;

public class MouseController : MonoBehaviour
{
    [Header("Sreen Controll")]
    [SerializeField] private float panSpeed = 30f;
    [SerializeField] private float zoomScale = 2.5f;

    [Space(10f)]
    [SerializeField] private float zoomInMax = 20f;
    [SerializeField] private float zoomOutMax = 52f;

    private CinemachineInputProvider inputProvider;
    private CinemachineVirtualCamera virtualCamera;
    private CinemachineConfiner2D confiner;
    private Transform cameraTransform;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Confined;

        inputProvider = GetComponent<CinemachineInputProvider>();
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        confiner = GetComponent<CinemachineConfiner2D>();
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

    private void FixedUpdate()
    {
        if (virtualCamera.transform.position != virtualCamera.State.CorrectedPosition)
            virtualCamera.transform.position = virtualCamera.State.CorrectedPosition;
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

    private void PanScreen(float x, float y)
    {
        Vector2 direction = PanDirection(x, y);
        cameraTransform.position = Vector3.Lerp(cameraTransform.position,
                                                cameraTransform.position + (Vector3)direction * panSpeed,
                                                Time.deltaTime);
    }

    private void ZoomScreen(float increment)
    {
        float fov = virtualCamera.m_Lens.OrthographicSize;
        float target = (increment > 0) ? fov + zoomScale : fov - zoomScale;

        virtualCamera.m_Lens.OrthographicSize = Mathf.Clamp(target, zoomInMax, zoomOutMax);
        
        confiner.InvalidateCache();
    }

    private void SetCameraPos(float x, float y)
    {
        virtualCamera.gameObject.transform.position = new Vector3(x / 2, y / 2, -10f);

        confiner.InvalidateCache();
    }
}