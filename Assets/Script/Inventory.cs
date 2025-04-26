using UnityEngine;

public class InventoryController : MonoBehaviour {
    public Sprite[] itemIcons; // Иконки предметов (1-7 слоты)
    public SpriteRenderer[] slots; // Ссылки на SpriteRenderer слотов (должно быть 8 элементов - 7 обычных + 1 с тремя точками)
    public Color activeSlotColor = Color.yellow; // Цвет активного слота
    public Color normalSlotColor = Color.white; // Обычный цвет слота
    public GameObject Inventar_0; // Ссылка на полноэкранный инвентарь
    
    private bool isInventoryOpen = false;
    private int currentSlot = 0; // Текущий выбранный слот (0-6 для обычных слотов)

    void Start() {
        // Автоназначение слотов если не заданы
        if (slots == null || slots.Length == 0)
        {
            slots = new SpriteRenderer[7];
            for (int i = 0; i < 7; i++)
            {
                var slotObj = transform.Find($"Slot_{i+1}")?.gameObject;
                if (slotObj != null) slots[i] = slotObj.GetComponent<SpriteRenderer>();
            }
        }
        UpdateSlotSelection();
        if (Inventar_0 != null) {
            Inventar_0.SetActive(false);
        }
    }

    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

            if (hit.collider != null) {
                for (int i = 0; i < slots.Length && i < 8; i++) {
                    if (slots[i] != null && hit.collider.gameObject == slots[i].gameObject) {
                        if (i == 7) {
                            ToggleFullscreenInventory();
                        }
                        else if (i < 7) { // Only allow 0-6
                            currentSlot = i;
                            UpdateSlotSelection();
                        }
                        break;
                    }
                }
            }
        }

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0 && !isInventoryOpen) {
            if (scroll > 0) {
                currentSlot = (currentSlot - 1 + 7) % 7;
            } else {
                currentSlot = (currentSlot + 1) % 7;
            }
            UpdateSlotSelection();
        }

        // Fixed digital keys handling (1-7 only)
        for (int i = 0; i < 7; i++) {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i) && !isInventoryOpen) {
                currentSlot = i;
                UpdateSlotSelection();
            }
        }
    }

    void ToggleFullscreenInventory() {
        isInventoryOpen = !isInventoryOpen;
        
        if (Inventar_0 != null) {
            Inventar_0.SetActive(isInventoryOpen);
        }
        
        for (int i = 0; i < 7 && i < slots.Length; i++) {
            if (slots[i] != null) {
                slots[i].gameObject.SetActive(!isInventoryOpen);
            }
        }
    }

    void UpdateSlotSelection() {
        for (int i = 0; i < 7 && i < slots.Length; i++) {
            if (slots[i] != null) {
                slots[i].color = (i == currentSlot) ? activeSlotColor : normalSlotColor;
            }
        }
        
        Debug.Log("Выбран слот " + (currentSlot + 1));
    }

    public Sprite GetSelectedItem() {
        if (currentSlot >= 0 && currentSlot < itemIcons.Length) {
            return itemIcons[currentSlot];
        }
        return null;
    }
}