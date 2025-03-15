using UnityEngine;

public class FarmGrid : MonoBehaviour
{
    public int gridSizeX = 10; // Ширина карты в блоках
    public int gridSizeY = 100; // Высота карты в блоках
    public float cellSize = 1f; // Размер одного блока
    public GameObject tilePrefab; // Префаб блока (квадрат или спрайт)

    private GameObject[,] grid; // Двумерный массив блоков

    private void Start()
    {
        GenerateGrid();
    }

    private void GenerateGrid()
    {
        grid = new GameObject[gridSizeX, gridSizeY];

        // Вычисляем центр карты
        Vector3 gridCenter = new Vector3(
            (gridSizeX - 1) * cellSize * 0.5f,
            (gridSizeY - 1) * cellSize * 0.5f,
            0
        );

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                // Создаем блок
                GameObject tile = Instantiate(tilePrefab, transform);

                // Вычисляем позицию блока относительно центра
                Vector3 tilePosition = new Vector3(
                    x * cellSize - gridCenter.x,
                    y * cellSize - gridCenter.y,
                    0
                );

                tile.transform.position = tilePosition;
                tile.name = $"Tile_{x}_{y}";

                // Сохраняем блок в массив
                grid[x, y] = tile;
            }
        }
    }

    // Метод для изменения состояния блока
    public void ChangeTileState(int x, int y, Color newColor)
    {
        if (x >= 0 && x < gridSizeX && y >= 0 && y < gridSizeY)
        {
            SpriteRenderer renderer = grid[x, y].GetComponent<SpriteRenderer>();
            if (renderer != null)
            {
                renderer.color = newColor;
            }
        }
    }
    public Vector2 GetGridBounds()
    {
        // Вычисляем границы карты
        float halfWidth = (gridSizeX * cellSize) / 2f;
        float halfHeight = (gridSizeY * cellSize) / 2f;

        // Возвращаем минимальные и максимальные границы
        return new Vector2(halfWidth, halfHeight);
    }
}