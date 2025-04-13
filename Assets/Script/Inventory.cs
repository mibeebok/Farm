using UnityEngine;

public class InventoryController : MonoBehaviour {
    public Sprite[] itemIcons; // Иконки предметов
    public SpriteRenderer[] slots; // Ссылки на SpriteRenderer слотов
    public Color activeSlotColor = Color.yellow; // Цвет активного слота
    public Color normalSlotColor = Color.white; // Обычный цвет слота
    
    private bool isInventoryOpen = false;
    private int currentSlot = 0; // Текущий выбранный слот (0-6)

    void Start() {
        UpdateSlotSelection(); // Инициализация выделения
    }

    void Update() {
        // Обработка клика по кнопке "..."
        if (Input.GetMouseButtonDown(0)) {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

            if (hit.collider != null && hit.collider.gameObject.name == "Slot_Dots") {
                ToggleInventory();
            }
        }

        // Обработка клика ЛКМ
        if (Input.GetMouseButtonDown(0)) {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

            if (hit.collider != null && hit.collider.gameObject.name == "Slot_Dots") {
                ToggleInventory();
            }
        }

        // Обработка прокрутки колесика
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0) {
            if (scroll > 0) {
                // Прокрутка вверх
                currentSlot = (currentSlot - 1 + 7) % 7;
            } else {
                // Прокрутка вниз
                currentSlot = (currentSlot + 1) % 7;
            }
            UpdateSlotSelection();
        }

        // Обработка цифровых клавиш 1-7
        for (int i = 0; i < 7; i++) {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i)) {
                currentSlot = i;
                UpdateSlotSelection();
            }
        }
    }

    void ToggleInventory() {
        isInventoryOpen = !isInventoryOpen;
        GetComponent<SpriteRenderer>().enabled = isInventoryOpen;
    }

    void UpdateSlotSelection() {
        // Обновляем цвет всех слотов
        for (int i = 0; i < slots.Length; i++) {
            slots[i].color = (i == currentSlot) ? activeSlotColor : normalSlotColor;
        }
        
        // Здесь можно добавить логику применения предмета
        Debug.Log("Выбран слот " + (currentSlot + 1));
    }

    // Метод для получения текущего выбранного предмета
    public Sprite GetSelectedItem() {
        if (currentSlot >= 0 && currentSlot < itemIcons.Length) {
            return itemIcons[currentSlot];
        }
        return null;
    }
}