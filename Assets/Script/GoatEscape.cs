using UnityEngine;

public class GoatEscape : MonoBehaviour
{
    [Header("Настройки побега")]
    [SerializeField] private float escapeSpeed = 3f; // Скорость движения
    [SerializeField] private float detectionRange = 5f; // Дистанция обнаружения игрока
    [SerializeField] private float screenMargin = 0.1f; // Отступ от края для деактивации

    private Transform player;
    private Vector2 escapeDirection;
    private bool isEscaping;
    private Camera mainCam;

    private void Start()
    {
        mainCam = Camera.main;
        player = GameObject.FindWithTag("Player")?.transform;
    }

    private void Update()
    {
        if (player == null) return;

        // Проверка дистанции до игрока
        if (Vector2.Distance(transform.position, player.position) < detectionRange)
        {
            if (!isEscaping)
            {
                CalculateEscapePath();
                isEscaping = true;
            }
            
            MoveOffScreen();
        }
        else
        {
            isEscaping = false;
        }
    }

    private void CalculateEscapePath()
    {
        // Выбираем направление к ближайшему краю
        Vector3 viewportPos = mainCam.WorldToViewportPoint(transform.position);
        
        if (viewportPos.x < 0.5f) // Если коза в левой части экрана
            escapeDirection = Vector2.left;
        else                      // Если в правой
            escapeDirection = Vector2.right;
    }

    private void MoveOffScreen()
    {
        // Плавное движение
        transform.Translate(escapeDirection * escapeSpeed * Time.deltaTime);

        // Проверка выхода за границы
        Vector3 viewportPos = mainCam.WorldToViewportPoint(transform.position);
        if (viewportPos.x < -screenMargin || 
            viewportPos.x > 1 + screenMargin ||
            viewportPos.y < -screenMargin || 
            viewportPos.y > 1 + screenMargin)
        {
            // Варианты действий при достижении края:
            // 1. Деактивировать объект
            gameObject.SetActive(false);
            
            // ИЛИ 2. Остановиться (закомментируйте строку выше)
            // escapeSpeed = 0;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}