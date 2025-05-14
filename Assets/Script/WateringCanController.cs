using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Animator))]
public class WateringCanController : MonoBehaviour
{
    [Header("Animation Settings")]
    [SerializeField] private float animationSpeed = 1f;
    [SerializeField] private string wateringTrigger = "Water";
    [SerializeField] private LayerMask farmLandLayer;
    
    [Header("Watering Settings")]
    [SerializeField] private float wateringCooldown = 0.5f;
    [SerializeField] private ParticleSystem waterParticles;
    
    private Camera mainCamera;
    private Animator animator;
    private bool isSelected = false;
    private float lastWateringTime;
    
    
    private void Awake()
    {
        mainCamera = Camera.main;
        animator = GetComponent<Animator>();
        // Добавьте проверку параметров
        if (animator != null)
        {
            bool hasWaterParam = false;
            foreach (var param in animator.parameters)
            {
                if (param.name == wateringTrigger && param.type == AnimatorControllerParameterType.Trigger)
                {
                    hasWaterParam = true;
                    break;
                }
            }
            
            if (!hasWaterParam)
            {
                Debug.LogError($"Animator parameter '{wateringTrigger}' not found or not a Trigger!");
            }
        }
        else
        {
            Debug.LogError("Animator component not found!");
        }
        animator.speed = animationSpeed;
        
        if (waterParticles != null)
            waterParticles.Stop();
    }
    
    private void Update()
    {
        if (!isSelected) return;
        
        if (Input.GetMouseButtonDown(0) && Time.time > lastWateringTime + wateringCooldown)
        {
            if (EventSystem.current.IsPointerOverGameObject()) return;
            
            Vector2 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero, Mathf.Infinity, farmLandLayer);
            
            if (hit.collider != null)
            {
                WaterLand(hit.point, hit.collider);
            }
        }
    }
    
    
    private void WaterLand(Vector2 position, Collider2D landCollider)
    {
        lastWateringTime = Time.time;
        
        // Проигрываем анимацию
        animator.SetTrigger(wateringTrigger);
        
        // Запускаем частицы
        if (waterParticles != null)
        {
            waterParticles.transform.position = position;
            waterParticles.Play();
        }
        
        // Поливаем землю (ищем FarmLand компонент)
        FarmLand land = landCollider.GetComponent<FarmLand>();
        if (land != null)
        {
            land.Water();
        }
    }
    
    public void SetSelected(bool selected)
    {
        isSelected = selected;
        
        // Визуальная обратная связь
        if (selected)
        {
            transform.localScale = Vector3.one * 1.1f;
        }
        else
        {
            transform.localScale = Vector3.one;
        }
    }
}