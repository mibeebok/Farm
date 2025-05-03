using UnityEngine;

public class InventoryController : MonoBehaviour {
    public Sprite[] itemIcons; // Иконки предметов (1-7 слоты)
    public SpriteRenderer[] slots; // Ссылки на SpriteRenderer слотов (8 элементов: 0-6 обычные, 7 - три точки)
    public Color activeSlotColor = Color.yellow;
    public Color normalSlotColor = Color.white;
    public GameObject Inventar_0; // Полноэкранный инвентарь
    
    private bool isInventoryOpen = false;
    private int currentSlot = 0;

    private Vector3 lastPlayerPosition;
    public GameObject Player;

    void Start() {
        // Автоназначение слотов
        if (slots == null || slots.Length == 0) {
            slots = new SpriteRenderer[8];
            for (int i = 0; i < 8; i++) {
                var slotObj = transform.Find($"Slot_{i+1}")?.gameObject;
                if (slotObj != null) {
                    slots[i] = slotObj.GetComponent<SpriteRenderer>();
                    //Коллайдер
                    if (slotObj.GetComponent<BoxCollider2D>() == null) {
                        slotObj.AddComponent<BoxCollider2D>();
                    }
                }
            }
        }
        CenterInventory();
        UpdateSlotSelection();
        if (Inventar_0 != null) Inventar_0.SetActive(false);

        //начальная позичия персонажа
        if(Player != null ) lastPlayerPosition = Player.transform.position;
    }

    void Update() {

        //прверка движения персонажа
        if(isInventoryOpen && Player != null){
            if(Vector3.Distance(Player.transform.position, lastPlayerPosition)>0.01f) {
                CloseInventory();
            }
            lastPlayerPosition = Player.transform.position;
        }

        // Клик ЛКМ
        if (Input.GetMouseButtonDown(0)) {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

            if (hit.collider != null) {
                for (int i = 0; i < slots.Length; i++) {
                    if (slots[i] != null && hit.collider.gameObject == slots[i].gameObject) {
                        if (i == 7) { // 8-й слот (индекс 7)
                            ToggleFullscreenInventory();
                        } else if (i < 7) { // Обычные слоты (0-6)
                            currentSlot = i;
                            UpdateSlotSelection();
                        }
                        break;
                    }
                }
            }
        }

        // Прокрутка колесика (только для слотов 0-6)
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0 && !isInventoryOpen) {
            currentSlot = scroll > 0 ? (currentSlot - 1 + 7) % 7 : (currentSlot + 1) % 7;
            UpdateSlotSelection();
        }

        // Цифровые клавиши 1-7
        for (int i = 0; i < 7; i++) {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i) && !isInventoryOpen) {
                currentSlot = i;
                UpdateSlotSelection();
            }
        }
    }

    void ToggleFullscreenInventory() {
        isInventoryOpen = !isInventoryOpen;
        if (Inventar_0 != null){
            CenterInventory();
            Inventar_0.SetActive(isInventoryOpen);
            
        } 
        
        // Отключаем/включаем видимость обычных слотов
        /*for (int i = 0; i < 7; i++) {
            if (slots[i] != null) slots[i].gameObject.SetActive(!isInventoryOpen);
        }*/
    }

    void UpdateSlotSelection() {
        for (int i = 0; i < 7; i++) {
            if (slots[i] != null) {
                slots[i].color = (i == currentSlot) ? activeSlotColor : normalSlotColor;
            }
        }
        Debug.Log($"Выбран слот {currentSlot + 1}");
    }

    public Sprite GetSelectedItem() {
        return (currentSlot >= 0 && currentSlot < itemIcons.Length) ? itemIcons[currentSlot] : null;
    }

    void CenterInventory() {
        if(Inventar_0 != null){
            Vector3 centerPosition = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 1.0f));
            centerPosition.z = 0;
            Inventar_0.transform.position = centerPosition;
        }
    }

    void CloseInventory() {
        if(isInventoryOpen){
            isInventoryOpen = false;
            if(Inventar_0 != null){
                Inventar_0.SetActive(false);
                Time.timeScale = 1;
            }
        }
    }

}