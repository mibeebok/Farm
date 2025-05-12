using UnityEngine;

public class GoatBehavior : Sounds
{
    [Header("Detection Settings")]
    [SerializeField] private float detectionRadius = 5f; // Радиус обнаружения игрока
    [SerializeField] private float soundPlayRadius = 3f; // Радиус для воспроизведения звука
    [SerializeField] private LayerMask playerLayer;
    
    [Header("Sound Settings")]
    [SerializeField] private float playInterval = 6f; // Интервал между звуками
    [SerializeField] private float minVolume = 0.1f; // Минимальная громкость
    [SerializeField] private float maxVolume = 0.3f; // Максимальная громкость
    
    [Header("Animation Settings")]
    [SerializeField] private Animator goatAnimator;
    [SerializeField] private string scaredParameter = "IsScared";
    
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
    }
    
    private void Update()
    {
        if (player == null) return;

        // Проверяем расстояние до игрока
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        isPlayerNear = distanceToPlayer <= detectionRadius;
        
        // Управляем анимацией
        goatAnimator.SetBool(scaredParameter, isPlayerNear);

        // Логика воспроизведения звука
        time += Time.deltaTime;
        
        if (time >= playInterval)
        {
            // Проверяем, находится ли игрок в радиусе звука
            if (distanceToPlayer <= soundPlayRadius)
            {
                // Рассчитываем громкость в зависимости от расстояния (чем ближе - тем громче)
                float volume = Mathf.Lerp(maxVolume, minVolume, 
                                       distanceToPlayer / soundPlayRadius);
                
                PlaySound(sounds[0], volume: volume, p1: 0.9f, p2: 1.3f);
            }
            time = 0;
        }
    }
    
    // Визуализация радиусов в редакторе
    private void OnDrawGizmosSelected()
    {
        // Радиус обнаружения (желтый)
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
        
        // Радиус звука (зеленый)
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, soundPlayRadius);
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