using UnityEngine;
using UnityEngine.EventSystems;

public class WateringCanController : MonoBehaviour
{
    [Header("Настройки")]
    [SerializeField] private int assignedSlotIndex = 0;
    [SerializeField] private LayerMask farmLandLayer;
    [SerializeField] private float wateringCooldown = 0.5f;
    [SerializeField] private Color wateredColor = new Color(0.5f, 0.8f, 1f); // Цвет политой земли
    
    [Header("Эффекты")]
    [SerializeField] private ParticleSystem waterParticles;
    [SerializeField] private AudioClip wateringSound;

    private FarmGrid farmGrid;
    private InventoryController inventory;
    private Camera mainCamera;
    private bool isSelected = false;
    private float lastWateringTime;
    private AudioSource audioSource;

    private void Awake()
    {
        mainCamera = Camera.main;
        inventory = FindObjectOfType<InventoryController>();
        farmGrid = FindObjectOfType<FarmGrid>();
        audioSource = GetComponent<AudioSource>() ?? gameObject.AddComponent<AudioSource>();
    }

    private void Update()
    {
        isSelected = inventory != null && inventory.GetSelectedSlot() == assignedSlotIndex;
        
        if (isSelected && Input.GetMouseButtonDown(0) 
            && !IsPointerOverUI() && CanWaterNow())
        {
            TryWatering();
        }
    }

    private bool CanWaterNow()
    {
        return Time.time > lastWateringTime + wateringCooldown;
    }

    private void TryWatering()
    {
        Vector2 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero, Mathf.Infinity, farmLandLayer);
        
        if (hit.collider != null && farmGrid != null)
        {
            lastWateringTime = Time.time;
            PlayWateringEffects(hit.point);
            WaterTile(hit.point);
        }
    }

    private void WaterTile(Vector2 worldPosition)
    {
        // Конвертируем мировые координаты в координаты сетки
        Vector3 gridCenter = new Vector3(
            (farmGrid.gridSizeX - 1) * farmGrid.cellSize * 0.5f,
            (farmGrid.gridSizeY - 1) * farmGrid.cellSize * 0.5f,
            0
        );

        // Вычисляем координаты тайла
        int x = Mathf.RoundToInt((worldPosition.x + gridCenter.x) / farmGrid.cellSize);
        int y = Mathf.RoundToInt((worldPosition.y + gridCenter.y) / farmGrid.cellSize);

        // Изменяем состояние тайла
        farmGrid.ChangeTileState(x, y, wateredColor);
    }

    private void PlayWateringEffects(Vector2 position)
    {
        if (waterParticles != null)
        {
            waterParticles.transform.position = position;
            waterParticles.Play();
        }
        
        if (wateringSound != null)
        {
            audioSource.PlayOneShot(wateringSound);
        }
    }

    private bool IsPointerOverUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }
}