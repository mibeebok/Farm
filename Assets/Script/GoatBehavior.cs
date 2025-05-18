using UnityEngine;

public class GoatBehavior : Sounds
{
    [Header("Detection Settings")]
    [SerializeField] private float detectionRadius = 5f;
    [SerializeField] private float soundPlayRadius = 3f;
    [SerializeField] private LayerMask playerLayer;
    
    [Header("Sound Settings")]
    [SerializeField] private float playInterval = 6f;
    [SerializeField] private float minVolume = 0.1f;
    [SerializeField] private float maxVolume = 0.3f;
    
    [Header("Animation Settings")]
    [SerializeField] private Animator goatAnimator;
    [SerializeField] private string scaredParameter = "IsScared";
    
    [Header("Respawn Settings")] 
    [SerializeField] private Vector2 spawnAreaMin; // Минимальные координаты зоны спавна
    [SerializeField] private Vector2 spawnAreaMax; // Максимальные координаты зоны спавна
    [SerializeField] private float minDistanceFromPlayer = 10f; // Минимальное расстояние от игрока
    
    private Transform player;
    private bool isPlayerNear;
    private float time = 0f;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        
        if (goatAnimator == null)
        {
            goatAnimator = GetComponent<Animator>();
            if (goatAnimator == null)
            {
                Debug.LogError("У козы не установлена анимация!");
            }
        }

        // Подписываемся на событие нового дня
        HouseController.OnNewDay += RespawnGoat;
    }

    private void OnDestroy()
    {
        // Отписываемся при уничтожении объекта
        HouseController.OnNewDay -= RespawnGoat;
    }

    private void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        isPlayerNear = distanceToPlayer <= detectionRadius;
        
        goatAnimator.SetBool(scaredParameter, isPlayerNear);

        time += Time.deltaTime;
        
        if (time >= playInterval)
        {
            if (distanceToPlayer <= soundPlayRadius)
            {
                float volume = Mathf.Lerp(maxVolume, minVolume, 
                                       distanceToPlayer / soundPlayRadius);
                
                PlaySound(sounds[0], volume: volume, p1: 0.9f, p2: 1.3f);
            }
            time = 0;
        }
    }

    private void RespawnGoat()
    {
        Vector2 newPosition;
        int attempts = 0;
        const int maxAttempts = 10;

        do
        {
            // Генерируем случайную позицию в заданной области
            newPosition = new Vector2(
                Random.Range(spawnAreaMin.x, spawnAreaMax.x),
                Random.Range(spawnAreaMin.y, spawnAreaMax.y)
            );

            attempts++;
            
            // Проверяем, чтобы коза не появилась слишком близко к игроку
            if (attempts >= maxAttempts || 
                Vector2.Distance(newPosition, player.position) >= minDistanceFromPlayer)
            {
                break;
            }
            
        } while (true);

        transform.position = newPosition;
        Debug.Log($"Коза переместилась на новую позицию: {newPosition}");
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
        
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, soundPlayRadius);
        
        // Визуализация зоны спавна
        Gizmos.color = new Color(1, 0.5f, 0, 0.3f);
        Vector3 center = (spawnAreaMin + spawnAreaMax) / 2;
        Vector3 size = spawnAreaMax - spawnAreaMin;
        Gizmos.DrawCube(center, size);
    }

    public void GoatScared()
    {
        PlaySound(sounds[1], volume: 0.5f);
    }
    
    public void GoatIdle()
    {
        PlaySound(sounds[0], volume: 0.2f, p1: 0.9f, p2: 1.1f);
    }
}