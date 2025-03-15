using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public FarmGrid farmGrid; // Ссылка на карту
    public float moveSpeed = 5f; // Скорость перемещения игрока

    private Vector2 gridBounds; // Границы карты

    private void Start()
    {
        // Получаем границы карты
        if (farmGrid != null)
        {
            gridBounds = farmGrid.GetGridBounds();
        }
        else
        {
            Debug.LogError("FarmGrid не назначен!");
        }
    }

    private void Update()
    {
        MovePlayer();
    }

    private void MovePlayer()
    {
        // Получаем ввод от игрока
        float moveX = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        float moveY = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;

        // Вычисляем новую позицию
        Vector3 newPosition = transform.position + new Vector3(moveX, moveY, 0);

        // Ограничиваем позицию игрока границами карты
        newPosition.x = Mathf.Clamp(newPosition.x, -gridBounds.x, gridBounds.x);
        newPosition.y = Mathf.Clamp(newPosition.y, -gridBounds.y, gridBounds.y);

        // Применяем новую позицию
        transform.position = newPosition;
    }
}