/*using UnityEngine;

public class WaterBounds : MonoBehaviour{
     [SerializeField] private GameObject waterTilePrefab; // Префаб спрайта воды (32x18)
    [SerializeField] private int tilesInLeg = 30; // Количество блоков в каждом катете
    [SerializeField] private float tileWidth = 32f; // Ширина тайла (в единицах Unity)
    [SerializeField] private float tileHeight = 18f; // Высота тайла (в единицах Unity)
    [SerializeField] private Vector2 startPosition = Vector2.zero; // Начальная позиция (нижний угол)

    private void Start()
    {
        GenerateWaterCorner();
    }

    private void GenerateWaterCorner()
    {
        // Генерация горизонтальной линии (нижняя часть угла)
        for (int x = 0; x < tilesInLeg; x++)
        {
            Vector2 position = startPosition + new Vector2(x * tileWidth, 0);
            Instantiate(waterTilePrefab, position, Quaternion.identity, transform);
        }

        // Генерация вертикальной линии (левая часть угла)
        for (int y = 1; y < tilesInLeg; y++) // y = 1, чтобы не дублировать первый блок
        {
            Vector2 position = startPosition + new Vector2(0, y * tileHeight);
            Instantiate(waterTilePrefab, position, Quaternion.identity, transform);
        }
    }
} */