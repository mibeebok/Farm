using UnityEngine;
using System.Collections.Generic;

public class InventoryController : MonoBehaviour
{
    [Header("Hands Animation")]
    public Animator handsAnimator;
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
    public static InventoryController Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }
    public void AddItem(Item item, int quantity = 1)
    {
        // Реализация добавления предмета в инвентарь
        // Например:
        if (item != null)
        {
            Debug.Log($"Added {quantity}x {item.name} to inventory");
            // Здесь должна быть ваша логика добавления предмета
        }
    }

    void Start()
    {
         SelectSlot(0); // Принудительно активируем первый слот
    
        InitializeHotbar();
        UpdateSlotVisuals();
        if (fullInventoryUI != null)
            fullInventoryUI.SetActive(false);

        if (fullInventoryUI != null)
            fullInventoryUI.SetActive(false);

        if (player != null)
            lastPlayerPosition = player.position;
    }

    void Update()
    {
        HandlePlayerMovement();
        HandleInput();
        HandleDragAndDrop();
    }

    void InitializeHotbar()
    {
        if (slotRenderers == null || slotRenderers.Length == 0)
        {
            slotRenderers = new SpriteRenderer[hotbarSize + 1];
            for (int i = 0; i <= hotbarSize; i++)
            {
                string slotName = i < hotbarSize ? $"Slot_{i + 1}" : "InventoryButton";
                Transform slot = transform.Find(slotName);
                if (slot != null)
                {
                    slotRenderers[i] = slot.GetComponent<SpriteRenderer>();
                    if (slot.GetComponent<BoxCollider2D>() == null)
                    {
                        var collider = slot.gameObject.AddComponent<BoxCollider2D>();
                        collider.size = new Vector2(1, 1);
                    }
                }
            }
        }
    }

    void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Tab)) // Добавьте альтернативную клавишу
        {
            ToggleInventory();
            return;
        }

        // Затем обработка слотов
        if (!isInventoryOpen)
        {
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            if (scroll != 0)
                SelectSlot((currentSlot + (scroll > 0 ? -1 : 1) + hotbarSize) % hotbarSize);

            for (int i = 0; i < hotbarSize; i++)
            {
                if (Input.GetKeyDown(KeyCode.Alpha1 + i))
                    SelectSlot(i);
            }
        }
        
        // Отдельная обработка кликов
        if (Input.GetMouseButtonDown(0))
            HandleMouseClick();
    }

    void HandlePlayerMovement()
    {
        if (isInventoryOpen && player != null &&
            Vector3.Distance(player.position, lastPlayerPosition) > 0.01f)
        {
            CloseInventory();
        }
        lastPlayerPosition = player.position;
    }

    void HandleMouseClick()
{
    if (currentDragItem != null) return;

    RaycastHit2D hit = GetRaycastHitAtMouse();
    if (hit.collider != null)
    {
        
        for (int i = 0; i < slotRenderers.Length; i++)
        {
            if (slotRenderers[i] != null && hit.collider.gameObject == slotRenderers[i].gameObject)
            {
                Debug.Log($"Slot {i} clicked"); // Отладочное сообщение
                
                if (i == hotbarSize)
                {
                    Debug.Log("Toggling inventory"); // Должно появиться при клике
                    ToggleInventory();
                }
                else
                {
                    SelectSlot(i);
                }
                break;
            }
        }
    }
}

    void HandleDragAndDrop()
    {
        if (Input.GetMouseButtonDown(0) && !isInventoryOpen)
            TryStartDrag();

        if (Input.GetMouseButton(0) && currentDragItem != null)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
            currentDragItem.transform.position = mousePos + new Vector3(0, dragOffset, 0);
        }

        if (Input.GetMouseButtonUp(0) && currentDragItem != null)
            EndDrag();
    }

    void TryStartDrag()
    {
        RaycastHit2D hit = GetRaycastHitAtMouse();
        if (hit.collider != null)
        {
            for (int i = 0; i < hotbarSize; i++)
            {
                if (slotRenderers[i] != null && hit.collider.gameObject == slotRenderers[i].gameObject)
                {
                    Item item = GetItemInSlot(i);
                    if (item != null && item.id != 0)
                    {
                        dragOriginSlot = i;
                        currentDragItem = Instantiate(dragItemPrefab);
                        currentDragItem.GetComponent<SpriteRenderer>().sprite = item.img;
                        return;
                    }
                }
            }
        }
    }

    void EndDrag()
    {
        RaycastHit2D hit = GetRaycastHitAtMouse();
        bool droppedOnSlot = false;

        if (hit.collider != null)
        {
            for (int i = 0; i < hotbarSize; i++)
            {
                if (slotRenderers[i] != null && hit.collider.gameObject == slotRenderers[i].gameObject)
                {
                    SwapSlots(dragOriginSlot, i);
                    droppedOnSlot = true;
                    break;
                }
            }
        }

        if (!droppedOnSlot && mainInventory != null && isInventoryOpen)
            MoveItemToMainInventory(dragOriginSlot);

        Destroy(currentDragItem);
        currentDragItem = null;
        dragOriginSlot = -1;
    }

    void MoveItemToMainInventory(int hotbarSlotIndex)
    {
        if (mainInventory == null || hotbarSlotIndex < 0 || hotbarSlotIndex >= hotbarSize)
            return;

        Item item = GetItemInSlot(hotbarSlotIndex);
        if (item == null || item.id == 0) return;

        if (mainInventory.AddItemToFirstFreeSlot(item, 1))
        {
            mainInventory.items[hotbarSlotIndex].count--;
            if (mainInventory.items[hotbarSlotIndex].count <= 0)
                mainInventory.items[hotbarSlotIndex].id = 0;

            UpdateSlotVisuals();
            mainInventory.UpdateInventory();
        }
    }

    void SwapSlots(int fromSlot, int toSlot)
    {
        if (mainInventory == null) return;

        (mainInventory.items[fromSlot], mainInventory.items[toSlot]) =
            (mainInventory.items[toSlot], mainInventory.items[fromSlot]);

        UpdateSlotVisuals();
        mainInventory.UpdateInventory();
    }

    void SelectSlot(int index)
    {
        currentSlot = index;
        UpdateSlotVisuals();
    }

    void UpdateSlotVisuals()
    {
        for (int i = 0; i < hotbarSize; i++)
        {
            if (slotRenderers[i] == null) continue;

            slotRenderers[i].color = (i == currentSlot) ? activeSlotColor : normalSlotColor;
            Item item = GetItemInSlot(i);
            slotRenderers[i].sprite = item?.img;
            slotRenderers[i].enabled = item != null;
        }
    }

    void ToggleInventory()
    {
        isInventoryOpen = !isInventoryOpen;

        if (fullInventoryUI != null)
        {
            fullInventoryUI.SetActive(isInventoryOpen);
            if (isInventoryOpen)
            {
                CenterInventory();
                mainInventory?.UpdateInventory();
            }
        }
    }

    void CenterInventory()
    {
        if (fullInventoryUI != null)
        {
            Vector3 centerPos = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 10f));
            centerPos.z = 0;
            fullInventoryUI.transform.position = centerPos;
        }
    }

    void CloseInventory()
    {
        if (!isInventoryOpen) return;

        isInventoryOpen = false;
        fullInventoryUI?.SetActive(false);
        Time.timeScale = 1f;
        UpdateSlotVisuals();
    }

    RaycastHit2D GetRaycastHitAtMouse()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        
        // Добавляем маску слоёв и максимальное расстояние
        int layerMask = LayerMask.GetMask("Hotbar"); // Создайте отдельный слой для хотбара
        float distance = 10f;
        
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero, distance, layerMask);  
        return hit;
    }

    Item GetItemInSlot(int slotIndex)
    {
        if (mainInventory != null && slotIndex < mainInventory.items.Count)
            return database.GetItemById(mainInventory.items[slotIndex].id);

        return null;
    }
    

    public Item GetSelectedItem() => GetItemInSlot(currentSlot);
    public int GetSelectedSlot() => currentSlot;
    public bool IsInventoryOpen() => isInventoryOpen;
}
