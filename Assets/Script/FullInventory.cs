using UnityEngine;

public class FullInventoryController : MonoBehaviour
{
    [Header("Main Settings")]
    public SpriteRenderer[] inventorySlots; // 32 SpriteRenderer'а для слотов
    public Sprite[] itemIcons; // Все возможные иконки предметов
    public Color normalColor = Color.white;
    public Color selectedColor = Color.yellow;
    
    [Header("Technical")]
    public KeyCode toggleKey = KeyCode.Tab;
    public bool pauseGameWhenOpen = true;
    
    private int selectedSlot = -1;
    private bool isInventoryOpen = false;

    void Start()
    {
        // Инициализация цветов
        ResetAllSlotsColors();
        SetInventoryVisible(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(toggleKey))
        {
            ToggleInventory();
        }

        if (isInventoryOpen)
        {
            HandleSlotSelection();
        }
    }

    void ToggleInventory()
    {
        isInventoryOpen = !isInventoryOpen;
        SetInventoryVisible(isInventoryOpen);
        
        if (pauseGameWhenOpen)
        {
            Time.timeScale = isInventoryOpen ? 0 : 1;
            Cursor.visible = isInventoryOpen;
            Cursor.lockState = isInventoryOpen ? CursorLockMode.None : CursorLockMode.Locked;
        }
    }

    void SetInventoryVisible(bool visible)
    {
        foreach (var slot in inventorySlots)
        {
            if (slot != null)
            {
                slot.gameObject.SetActive(visible);
            }
        }
    }

    void HandleSlotSelection()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

            if (hit.collider != null)
            {
                for (int i = 0; i < inventorySlots.Length; i++)
                {
                    if (inventorySlots[i] != null && hit.collider.gameObject == inventorySlots[i].gameObject)
                    {
                        SelectSlot(i);
                        break;
                    }
                }
            }
        }
    }

    void SelectSlot(int slotIndex)
    {
        // Сбрасываем предыдущий выбор
        if (selectedSlot >= 0 && selectedSlot < inventorySlots.Length)
        {
            inventorySlots[selectedSlot].color = normalColor;
        }

        // Устанавливаем новый выбор
        if (slotIndex >= 0 && slotIndex < inventorySlots.Length)
        {
            selectedSlot = slotIndex;
            inventorySlots[slotIndex].color = selectedColor;
            Debug.Log($"Selected inventory slot: {slotIndex + 1}");
            
            // Здесь можно добавить логику использования предмета
            // UseSelectedItem();
        }
    }

    void ResetAllSlotsColors()
    {
        foreach (var slot in inventorySlots)
        {
            if (slot != null)
            {
                slot.color = normalColor;
            }
        }
    }

    // Пример метода для использования предмета
    void UseSelectedItem()
    {
        if (selectedSlot >= 0 && itemIcons.Length > 0)
        {
            // Здесь логика использования предмета
            Debug.Log($"Using item from slot {selectedSlot + 1}");
        }
    }
}