using UnityEngine;

public class PlayerBounds : MonoBehaviour
{
    public Transform mapTransform; // Transform карты
    public Vector2 mapSize; // Размеры карты вручную (ширина и высота)

    private Vector2 minBounds;
    private Vector2 maxBounds;

    private void Start()
    {
        CalculateBounds();
    }

    private void CalculateBounds()
    {
        if (mapTransform != null)
        {
            // Получаем позицию центра карты
            Vector3 mapCenter = mapTransform.position;

            // Рассчитываем границы на основе заданных размеров
            minBounds = new Vector2(mapCenter.x - mapSize.x / 2, mapCenter.y - mapSize.y / 2);
            maxBounds = new Vector2(mapCenter.x + mapSize.x / 2, mapCenter.y + mapSize.y / 2);
        }
        else
        {
            Debug.LogError("mapTransform не назначен!");
        }
    }

    private void Update()
    {
        Vector3 currentPosition = transform.position;
        currentPosition.x = Mathf.Clamp(currentPosition.x, minBounds.x, maxBounds.x);
        currentPosition.y = Mathf.Clamp(currentPosition.y, minBounds.y, maxBounds.y);
        transform.position = currentPosition;
    }
}