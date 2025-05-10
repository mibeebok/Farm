using UnityEngine;

public class GoatBehavior : MonoBehaviour
{
    [Header("Detection Settings")]
    [SerializeField] private float detectionRadius = 5f;
    [SerializeField] private LayerMask playerLayer;
    
    [Header("Animation Settings")]
    [SerializeField] private Animator goatAnimator;
    [SerializeField] private string scaredParameter = "IsScared";
    
    private Transform player;
    private bool isPlayerNear;
    
    private void Start()
    {
        // Находим игрока по тегу
        player = GameObject.FindGameObjectWithTag("Player").transform;
        
        // Проверяем наличие компонентов
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
        // Проверяем расстояние до игрока
        if (player != null)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);
            isPlayerNear = distanceToPlayer <= detectionRadius;
            
            // Управляем анимацией
            goatAnimator.SetBool(scaredParameter, isPlayerNear);
        }
    }
    
    // Визуализация радиуса обнаружения в редакторе
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}