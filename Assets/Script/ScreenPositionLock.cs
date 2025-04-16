using UnityEngine;

public class ScreenPositionLock : MonoBehaviour 
{
    public Camera uiCamera; // Назначьте основную камеру
    public Vector2 screenPosition = new Vector2(0.5f, 0.1f); // Координаты (0-1)

    void Update()
    {
        if (uiCamera == null) uiCamera = Camera.main;
        
        // Конвертируем экранные координаты в мировые
        Vector3 worldPos = uiCamera.ViewportToWorldPoint(
            new Vector3(screenPosition.x, screenPosition.y, 10)
        );
        
        transform.position = new Vector3(worldPos.x, worldPos.y, 0);
    }
}