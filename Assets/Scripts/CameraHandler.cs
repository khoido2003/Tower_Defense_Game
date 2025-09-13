using Cinemachine;
using UnityEngine;

public class CameraHandler : MonoBehaviour
{
    [SerializeField]
    private CinemachineVirtualCamera cinemachineVirtualCamera;

    private float orthographicSize;
    private float targetOrthographicSize;
    private Vector2? lastTouchPosition;
    private float lastPinchDistance;

    private void Start()
    {
        orthographicSize = cinemachineVirtualCamera.m_Lens.OrthographicSize;
        targetOrthographicSize = orthographicSize;
    }

    private void Update()
    {
#if UNITY_EDITOR
        // In Unity Editor (including simulator), always allow both touch and keyboard/mouse
        if (Input.touchCount > 0)
        {
            HandleTouchMovement();
            HandleTouchZoom();
        }
        
        // Always allow keyboard/mouse in editor for testing
        HandleKeyboardMovement();
        HandleMouseZoom();
#else
        // On actual device builds
        if (Application.isMobilePlatform)
        {
            HandleTouchMovement();
            HandleTouchZoom();
        }
        else
        {
            HandleKeyboardMovement();
            HandleMouseZoom();
        }
#endif
    }

    private void HandleKeyboardMovement()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxis("Vertical");

        Vector3 moveDir = new Vector3(x, y).normalized;
        float moveSpeed = 30f;

        transform.position += moveDir * moveSpeed * Time.deltaTime;
    }

    private void HandleMouseZoom()
    {
        float zoomAmount = 2f;
        targetOrthographicSize -= Input.mouseScrollDelta.y * zoomAmount;
        ApplyZoom();
    }

    private void HandleTouchMovement()
    {
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);
            
            // Only handle movement if we're not over a UI element
            if (!UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject(touch.fingerId))
            {
                if (touch.phase == TouchPhase.Began)
                {
                    lastTouchPosition = touch.position;
                }
                else if (touch.phase == TouchPhase.Moved && lastTouchPosition.HasValue)
                {
                    Vector2 touchDelta = touch.position - lastTouchPosition.Value;
                    // Adjust the sensitivity for simulator and real device
                    float sensitivity = 0.05f;
                    #if UNITY_EDITOR
                    sensitivity = 0.1f; // Higher sensitivity in simulator
                    #endif
                    
                    Vector3 moveDir = new Vector3(-touchDelta.x, -touchDelta.y, 0) * sensitivity;
                    float moveSpeed = 15f; // Reduced for smoother movement
                    transform.position += moveDir * moveSpeed * Time.deltaTime;
                    lastTouchPosition = touch.position;
                }
            }
        }
        else
        {
            lastTouchPosition = null;
        }
    }

    private void HandleTouchZoom()
    {
        if (Input.touchCount == 2)
        {
            Touch touch0 = Input.GetTouch(0);
            Touch touch1 = Input.GetTouch(1);

            // Only handle zoom if neither touch is over UI
            if (!UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject(touch0.fingerId) &&
                !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject(touch1.fingerId))
            {
                if (touch0.phase == TouchPhase.Began || touch1.phase == TouchPhase.Began)
                {
                    lastPinchDistance = Vector2.Distance(touch0.position, touch1.position);
                }
                else if (touch0.phase == TouchPhase.Moved || touch1.phase == TouchPhase.Moved)
                {
                    float currentPinchDistance = Vector2.Distance(touch0.position, touch1.position);
                    float pinchDelta = currentPinchDistance - lastPinchDistance;
                    
                    // Adjust sensitivity for simulator and real device
                    float sensitivity = 0.02f;
                    #if UNITY_EDITOR
                    sensitivity = 0.04f; // Higher sensitivity in simulator
                    #endif
                    
                    targetOrthographicSize -= pinchDelta * sensitivity;
                    lastPinchDistance = currentPinchDistance;
                    
                    ApplyZoom();
                }
            }
        }
    }

    private void ApplyZoom()
    {
        float orthographicSizeMin = 10;
        float orthographicSizeMax = 30;

        targetOrthographicSize = Mathf.Clamp(targetOrthographicSize, orthographicSizeMin, orthographicSizeMax);

        float zoomSpeed = 5f;
        orthographicSize = Mathf.Lerp(orthographicSize, targetOrthographicSize, Time.deltaTime * zoomSpeed);

        cinemachineVirtualCamera.m_Lens.OrthographicSize = orthographicSize;
    }
}
