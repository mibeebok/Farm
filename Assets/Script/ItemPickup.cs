using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public CropType cropType = CropType.Potato;
    public bool isSeed = true;
    public int amount = 1;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T)) // Тестовая клавиша
        {
            Item testItem = DataBase.Instance.GetItemByCropType(CropType.Potato, true);
            InventoryController.Instance.AddItem(testItem, 1);
            Debug.Log("Test item added");
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (isSeed)
            {
                if (InventoryManager.Instance != null)
                {
                    InventoryManager.Instance.AddSeedToInventory(cropType, amount);
                    Debug.Log($"Picked up {amount} {cropType} seeds");
                }
                else
                {
                    Debug.LogError("InventoryManager instance is missing!");
                }
            }
            else
            {
                InventoryManager.Instance?.AddVegetableToInventory(cropType, amount);
            }
            
            Destroy(gameObject);
        }
    }
}