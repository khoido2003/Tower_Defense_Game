using UnityEngine;

public class UtilsClass : MonoBehaviour
{
    private static Camera mainCamera;

    public static Vector3 GetMouseWorldPosition()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;

        Vector3 position;
        
        if (Application.isMobilePlatform && Input.touchCount > 0)
        {
            // Use the first touch position for mobile
            Touch touch = Input.GetTouch(0);
            Vector3 touchPos = new Vector3(touch.position.x, touch.position.y, 0);
            
            // Check if the touch position is within the screen bounds
            if (touchPos.x >= 0 && touchPos.x <= Screen.width && 
                touchPos.y >= 0 && touchPos.y <= Screen.height)
            {
                touchPos.z = mainCamera.nearClipPlane;
                position = mainCamera.ScreenToWorldPoint(touchPos);
            }
            else
            {
                // Return the last valid position or center of the screen if touch is out of bounds
                touchPos = new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, mainCamera.nearClipPlane);
                position = mainCamera.ScreenToWorldPoint(touchPos);
            }
        }
        else
        {
            // Use mouse position for PC
            Vector3 mousePos = Input.mousePosition;
            
            // Check if the mouse position is within the screen bounds
            if (mousePos.x >= 0 && mousePos.x <= Screen.width && 
                mousePos.y >= 0 && mousePos.y <= Screen.height)
            {
                mousePos.z = mainCamera.nearClipPlane;
                position = mainCamera.ScreenToWorldPoint(mousePos);
            }
            else
            {
                // Return the last valid position or center of the screen if mouse is out of bounds
                mousePos = new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, mainCamera.nearClipPlane);
                position = mainCamera.ScreenToWorldPoint(mousePos);
            }
        }

        position.z = 0f;
        return position;
    }

    public static Vector3 GetRandomDir()
    {
        return new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
    }

    public static float GetAngleFromVector(Vector3 vector)
    {
        float radians = Mathf.Atan2(vector.y, vector.x);
        float degrees = radians * Mathf.Rad2Deg;

        return degrees;
    }
}
