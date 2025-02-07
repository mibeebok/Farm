using UnityEngine;

public class PlayerBounds : MonoBehaviour
{
 public SpriteRenderer mapRenderer; // Спрайт карты
    private Vector2 minBounds;
    private Vector2 maxBounds;

    private void Start()
    {
        if (mapRenderer != null)
        {
            // Получаем размеры карты
            float mapWidth = mapRenderer.bounds.size.x;
            float mapHeight = mapRenderer.bounds.size.y;

            // Получаем позицию центра карты
            Vector3 mapCenter = mapRenderer.transform.position;

            // Рассчитываем границы
            minBounds = new Vector2(mapCenter.x - mapWidth / 2, mapCenter.y - mapHeight / 2);
            maxBounds = new Vector2(mapCenter.x + mapWidth / 2, mapCenter.y + mapHeight / 2);
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
