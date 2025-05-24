using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public int itemId;
    public int quantity = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Item item = DataBase.Instance.GetItemById(itemId);
            if (item != null && InventoryController.Instance.AddItem(item, quantity))
            {
                Destroy(gameObject);
            }
        }
    }
}