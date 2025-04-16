using UnityEngine;

public class PauseButtonPosition : MonoBehaviour 
{
    [Header("Настройки")]
    public Camera uiCamera;
    public Vector2 screenPosition = new Vector2(0.95f, 0.9f); // Право-верх
    public float pixelOffsetX = -50f;
    public float pixelOffsetY = -50f;

    void Update()
    {
        UpdatePosition();
        
        if (Input.GetMouseButtonDown(0))
        {
            CheckClick();
        }
    }

    void UpdatePosition()
    {
        if (uiCamera == null) uiCamera = Camera.main;
        
        Vector3 viewportPos = new Vector3(screenPosition.x, screenPosition.y, 10);
        Vector3 worldPos = uiCamera.ViewportToWorldPoint(viewportPos);
        
        // Конвертируем пиксельные отступы в мировые единицы
        float ppu = 100f; // Пикселей на единицу (подберите под ваш проект)
        Vector3 offset = new Vector3(pixelOffsetX/ppu, pixelOffsetY/ppu, 0);
        
        transform.position = worldPos + offset;
    }

    void CheckClick()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (GetComponent<Collider2D>().OverlapPoint(mousePos))
        {
            // Ваш код паузы
            Debug.Log("Кнопка паузы нажата!");
        }
    }
}