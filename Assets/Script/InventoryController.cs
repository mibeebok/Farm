using UnityEngine;
using System.Collections.Generic;

public class InventoryController : MonoBehaviour 
{
    [Header("Hotbar Settings")]
    public int hotbarSize = 7;
    public SpriteRenderer[] slotRenderers;
    public Color activeSlotColor = Color.yellow;
    public Color normalSlotColor = Color.white;
    
    [Header("Inventory References")]
    public GameObject fullInventoryUI;
    public Inventory mainInventory;
    public DataBase database;
    public Transform player;
    
    [Header("Drag & Drop")]
    public GameObject dragItemPrefab;
    public float dragOffset = 1f;

    private int currentSlot = 0;
    private bool isInventoryOpen = false;
    private Vector3 lastPlayerPosition;
    private GameObject currentDragItem;
    private int dragOriginSlot = -1;

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
        if(slotRenderers == null || slotRenderers.Length == 0)
        {
            slotRenderers = new SpriteRenderer[hotbarSize + 1];
            
            for(int i = 0; i <= hotbarSize; i++)
            {
                string slotName = i < hotbarSize ? $"Slot_{i+1}" : "InventoryButton";
                Transform slot = transform.Find(slotName);
                
                if(slot != null)
                {
                    slotRenderers[i] = slot.GetComponent<SpriteRenderer>();
                    
                    if(slot.GetComponent<BoxCollider2D>() == null)
                    {
                        var collider = slot.gameObject.AddComponent<BoxCollider2D>();
                        collider.size = new Vector2(1, 1);
                    }
                }
            }
        }
    }

    void Update() 
    {
        HandlePlayerMovement();
        HandleInput();
        HandleDragAndDrop();
    }

    void HandleDragAndDrop()
    {
        if(Input.GetMouseButtonDown(0) && !isInventoryOpen)
        {
            StartDrag();
        }
        
        if(Input.GetMouseButton(0) && currentDragItem != null)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
            currentDragItem.transform.position = mousePos + new Vector3(0, dragOffset, 0);
        }
        
        if(Input.GetMouseButtonUp(0) && currentDragItem != null)
        {
            EndDrag();
        }
    }

    void StartDrag()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);
        
        if(hit.collider != null)
        {
            for(int i = 0; i < hotbarSize; i++)
            {
                if(slotRenderers[i] != null && hit.collider.gameObject == slotRenderers[i].gameObject)
                {
                    Item item = GetItemInSlot(i);
                    if(item != null && item.id != 0)
                    {
                        dragOriginSlot = i;
                        StartDragItem(item.img);
                        return;
                    }
                }
            }
        }
    }

   void EndDrag()
{
    Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);
    bool droppedOnSlot = false;

    if(hit.collider != null)
    {
        for(int i = 0; i < hotbarSize; i++)
        {
            if(slotRenderers[i] != null && hit.collider.gameObject == slotRenderers[i].gameObject)
            {
                SwapSlots(dragOriginSlot, i);
                droppedOnSlot = true;
                break;
            }
        }
    }

    if(!droppedOnSlot && mainInventory != null && isInventoryOpen)
    {
        MoveItemToMainInventory(dragOriginSlot); // Используем восстановленный метод
    }

    Destroy(currentDragItem);
    currentDragItem = null;
    dragOriginSlot = -1;
}
    // Добавьте этот метод в ваш InventoryController
public void MoveItemToMainInventory(int hotbarSlotIndex)
{
    if (mainInventory == null || hotbarSlotIndex < 0 || hotbarSlotIndex >= hotbarSize) 
        return;
    
    // Получаем предмет из хотбара
    Item item = database.GetItemById(mainInventory.items[hotbarSlotIndex].id);
    if (item == null || item.id == 0) return; // Проверка на пустой слот

    // Пытаемся добавить в основной инвентарь
    if (mainInventory.AddItemToFirstFreeSlot(item, 1))
    {
        // Уменьшаем количество в хотбаре
        mainInventory.items[hotbarSlotIndex].count--;
        
        // Если предметов не осталось - очищаем слот
        if (mainInventory.items[hotbarSlotIndex].count <= 0)
        {
            mainInventory.items[hotbarSlotIndex].id = 0;
        }
        
        UpdateSlotVisuals();
        mainInventory.UpdateInventory();
    }
}



    void SwapSlots(int fromSlot, int toSlot)
    {
        if(mainInventory == null) return;
        
        // Временное хранение предметов
        int tempId = mainInventory.items[fromSlot].id;
        int tempCount = mainInventory.items[fromSlot].count;
        
        // Перемещение
        mainInventory.items[fromSlot].id = mainInventory.items[toSlot].id;
        mainInventory.items[fromSlot].count = mainInventory.items[toSlot].count;
        
        mainInventory.items[toSlot].id = tempId;
        mainInventory.items[toSlot].count = tempCount;
        
        UpdateSlotVisuals();
        mainInventory.UpdateInventory();
    }

    void StartDragItem(Sprite sprite)
    {
        if(dragItemPrefab != null && sprite != null)
        {
            currentDragItem = Instantiate(dragItemPrefab);
            currentDragItem.GetComponent<SpriteRenderer>().sprite = sprite;
        }
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
        if(Input.GetMouseButtonDown(0))
        {
            HandleMouseClick();
        }

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if(scroll != 0 && !isInventoryOpen)
        {
            currentSlot = (currentSlot + (scroll > 0 ? -1 : 1) + hotbarSize) % hotbarSize;
            UpdateSlotVisuals();
        }

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
        if(currentDragItem != null) return;
        
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

        if(hit.collider != null)
        {
            for(int i = 0; i < slotRenderers.Length; i++)
            {
                if(slotRenderers[i] != null && hit.collider.gameObject == slotRenderers[i].gameObject)
                {
                    if(i == hotbarSize)
                    {
                        ToggleInventory();
                    }
                    else
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
        for(int i = 0; i < hotbarSize; i++)
        {
            if(slotRenderers[i] != null)
            {
                slotRenderers[i].color = i == currentSlot ? activeSlotColor : normalSlotColor;
                
                if(mainInventory != null && i < mainInventory.items.Count)
                {
                    Item item = database.GetItemById(mainInventory.items[i].id);
                    slotRenderers[i].sprite = item?.img;
                    slotRenderers[i].enabled = item != null;
                }
            }
        }
    }

    Item GetItemInSlot(int slotIndex)
    {
        if(mainInventory != null && slotIndex < mainInventory.items.Count)
        {
            return database.GetItemById(mainInventory.items[slotIndex].id);
        }
        return null;
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
        if(mainInventory != null && currentSlot >= 0 && currentSlot < mainInventory.items.Count)
        {
            return database.GetItemById(mainInventory.items[currentSlot].id);
        }
        return null;
    }
}