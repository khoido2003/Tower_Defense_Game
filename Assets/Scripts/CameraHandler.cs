using Cinemachine;
using UnityEngine;

public class CameraHandler : MonoBehaviour
{
    [SerializeField]
    private CinemachineVirtualCamera cinemachineVirtualCamera;

    private float orthographicSize;
    private float targetOrthographicSize;

    private void Start()
    {
        orthographicSize = cinemachineVirtualCamera.m_Lens.OrthographicSize;

        targetOrthographicSize = orthographicSize;
    }

    private void Update()
    {
        HandleMovement();
        HandleZoom();
    }

    private void HandleMovement()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxis("Vertical");

        Vector3 moveDir = new Vector3(x, y).normalized;
        float moveSpeed = 30f;

        transform.position += moveDir * moveSpeed * Time.deltaTime;
    }

    private void HandleZoom()
    {
        float zoomAmount = 2f;
        targetOrthographicSize -= Input.mouseScrollDelta.y * zoomAmount;

        float orthographicSizeMin = 10;
        float orthographicSizeMax = 30;

        targetOrthographicSize = Mathf.Clamp(
            targetOrthographicSize,
            orthographicSizeMin,
            orthographicSizeMax
        );

        float zoomSpeed = 5f;
        orthographicSize = Mathf.Lerp(
            orthographicSize,
            targetOrthographicSize,
            Time.deltaTime * zoomSpeed
        );

        cinemachineVirtualCamera.m_Lens.OrthographicSize = orthographicSize;
    }
}
