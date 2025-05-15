using UnityEngine;

public class WateringCanController : MonoBehaviour
{
    [Header("References")]
    public InventoryController inventoryController;
    public Animator handsAnimator;

    [Header("Watering Settings")]
    public int wateringCanItemId = 1; // ID лейки в базе данных
    public string wateringTrigger = "Water"; // Имя триггера в Animator
    public float soilWateringDelay = 0.4f;

    void Update()
    {
        // Проверяем, выбран ли нужный слот и нажата ЛКМ
        if (Input.GetMouseButtonDown(0))
        {
            TryWaterSoil();
        }
        if (Input.GetMouseButtonDown(0))
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

        if (hit.collider != null)
        {
            Debug.Log("Попали в объект: " + hit.collider.name);

            if (hit.collider.CompareTag("Soil"))
            {
                Debug.Log("Клик по земле!");

                // Тест анимации
                if (handsAnimator != null)
                    handsAnimator.SetTrigger("Water");

                // Тест полива
                var soil = hit.collider.GetComponent<SoilTile>();
                if (soil != null)
                    soil.Water();
            }
        }
    }
    }

    void TryWaterSoil()
    {
        if (inventoryController == null) return;

        // Только если выбран слот 0 и в нём лейка
        if (inventoryController.GetSelectedSlot() != 0) return;

        Item selectedItem = inventoryController.GetSelectedItem();
        if (selectedItem == null || selectedItem.id != wateringCanItemId) return;

        // Наводим луч на землю
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

        if (hit.collider != null && hit.collider.CompareTag("Soil"))
        {
            // Анимация полива
            if (handsAnimator != null)
            {
                handsAnimator.SetTrigger(wateringTrigger);
            }

            // Увлажнение земли
            SoilTile soil = hit.collider.GetComponent<SoilTile>();
            if (soil != null)
            {
                StartCoroutine(soil.WaterWithDelay(soilWateringDelay));
            }
        }
    }
}
