using UnityEngine;

public class InventoryController : MonoBehaviour 
{
    [Header("Hotbar Settings")]
    public int hotbarSize = 7; // Количество слотов быстрого доступа
    public SpriteRenderer[] slotRenderers; // Спрайты слотов
    public Color activeSlotColor = Color.yellow;
    public Color normalSlotColor = Color.white;
    
    [Header("Inventory References")]
    public GameObject fullInventoryUI; // Полноэкранный инвентарь
    public Inventory mainInventory; // Ссылка на основной инвентарь
    public Transform player; // Ссылка на игрока

    [Header("Item Icons")]
    public Sprite[] itemIcons; // Альтернативные иконки предметов (если не используется mainInventory)

    private int currentSlot = 0;
    private bool isInventoryOpen = false;
    private Vector3 lastPlayerPosition;

    void Start() 
    {
        InitializeHotbar();
        UpdateSlotVisuals();
        
        if(fullInventoryUI != null) 
            fullInventoryUI.SetActive(false);
            
        if(player != null) 
            lastPlayerPosition = player.position;
    }

    void InitializeHotbar()
    {
        // Автоинициализация если не настроено вручную
        if(slotRenderers == null || slotRenderers.Length == 0)
        {
            slotRenderers = new SpriteRenderer[hotbarSize + 1]; // +1 для кнопки инвентаря
            
            for(int i = 0; i <= hotbarSize; i++)
            {
                Transform slot = transform.Find($"Slot_{i+1}");
                if(slot != null)
                {
                    slotRenderers[i] = slot.GetComponent<SpriteRenderer>();
                    
                    // Добавляем коллайдер если нет
                    if(slot.GetComponent<BoxCollider2D>() == null)
                    {
                        slot.gameObject.AddComponent<BoxCollider2D>();
                    }
                }
            }
        }
    }

    void Update() 
    {
        HandlePlayerMovement();
        HandleInput();
    }
     public void UpdateHotbarVisual(int slotId, int itemId, int count)
    {
        if(slotId < 0 || slotId >= slotRenderers.Length || slotRenderers[slotId] == null) 
            return;
        
        Item item = mainInventory.data.GetItemById(itemId);
        slotRenderers[slotId].sprite = item?.img;
    }

    void HandlePlayerMovement()
    {
        if(isInventoryOpen && player != null && 
           Vector3.Distance(player.position, lastPlayerPosition) > 0.01f)
        {
            CloseInventory();
        }
        lastPlayerPosition = player.position;
    }

    void HandleInput()
    {
        // Клик ЛКМ
        if(Input.GetMouseButtonDown(0))
        {
            HandleMouseClick();
        }

        // Прокрутка колесика
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if(scroll != 0 && !isInventoryOpen)
        {
            currentSlot = (currentSlot + (scroll > 0 ? -1 : 1) + hotbarSize) % hotbarSize;
            UpdateSlotVisuals();
        }

        // Цифровые клавиши 1-7
        for(int i = 0; i < hotbarSize; i++)
        {
            if(Input.GetKeyDown(KeyCode.Alpha1 + i) && !isInventoryOpen)
            {
                currentSlot = i;
                UpdateSlotVisuals();
            }
        }
    }

    void HandleMouseClick()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

        if(hit.collider != null)
        {
            for(int i = 0; i < slotRenderers.Length; i++)
            {
                if(slotRenderers[i] != null && hit.collider.gameObject == slotRenderers[i].gameObject)
                {
                    if(i == hotbarSize) // Последний слот - кнопка инвентаря
                    {
                        ToggleInventory();
                    }
                    else if(i < hotbarSize) // Обычные слоты
                    {
                        currentSlot = i;
                        UpdateSlotVisuals();
                    }
                    break;
                }
            }
        }
    }

    void UpdateSlotVisuals()
    {
        // Обновляем цвета выбранного слота
        for(int i = 0; i < hotbarSize; i++)
        {
            if(slotRenderers[i] != null)
            {
                slotRenderers[i].color = i == currentSlot ? activeSlotColor : normalSlotColor;
            }
        }

        // Обновляем иконки предметов
        if(mainInventory != null)
        {
            // Из основного инвентаря
            for(int i = 0; i < hotbarSize; i++)
            {
                if(i < mainInventory.items.Count && slotRenderers[i] != null)
                {
                    Item item = mainInventory.data.GetItemById(mainInventory.items[i].id);
                    slotRenderers[i].sprite = item?.img;
                }
            }
        }
        else if(itemIcons != null)
        {
            // Из локального массива иконок
            for(int i = 0; i < Mathf.Min(hotbarSize, itemIcons.Length); i++)
            {
                if(slotRenderers[i] != null)
                {
                    slotRenderers[i].sprite = itemIcons[i];
                }
            }
        }
    }

    void ToggleInventory()
    {
        isInventoryOpen = !isInventoryOpen;
        
        if(fullInventoryUI != null)
        {
            fullInventoryUI.SetActive(isInventoryOpen);
            if(isInventoryOpen)
            {
                CenterInventory();
                if(mainInventory != null)
                    mainInventory.UpdateInventory();
            }
        }
    }

    void CenterInventory()
    {
        if(fullInventoryUI != null)
        {
            Vector3 centerPos = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 10f));
            centerPos.z = 0;
            fullInventoryUI.transform.position = centerPos;
        }
    }

    void CloseInventory()
    {
        if(isInventoryOpen)
        {
            isInventoryOpen = false;
            if(fullInventoryUI != null)
                fullInventoryUI.SetActive(false);
            
            Time.timeScale = 1f;
            UpdateSlotVisuals();
        }
    }

    public Item GetSelectedItem()
    {
        if(mainInventory != null)
        {
            // Получаем из основного инвентаря
            if(currentSlot >= 0 && currentSlot < mainInventory.items.Count)
                return mainInventory.data.GetItemById(mainInventory.items[currentSlot].id);
        }
        else if(itemIcons != null && currentSlot >= 0 && currentSlot < itemIcons.Length)
        {
            // Создаем временный объект Item для локальных иконок
            return new Item() { img = itemIcons[currentSlot] };
        }
        
        return null;
    }
}