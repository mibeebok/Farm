using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddSeedToInventory(CropType cropType, int amount)
    {
        Item seedItem = DataBase.Instance.GetItemByCropType(cropType, isSeed: true);
        if (seedItem != null)
        {
            InventoryController.Instance.AddItem(seedItem, amount);
            Debug.Log($"Added {amount} {seedItem.name} to inventory");
        }
        else
        {
            Debug.LogError($"Seed item not found for {cropType}");
        }
    }

    public void AddVegetableToInventory(CropType cropType, int amount)
    {
        Item vegItem = DataBase.Instance.GetItemByCropType(cropType, isSeed: false);
        if (vegItem != null)
        {
            InventoryController.Instance.AddItem(vegItem, amount);
            Debug.Log($"Added {amount} {vegItem.name} to inventory");
        }
        else
        {
            Debug.LogError($"Vegetable item not found for {cropType}");
        }
    }
}