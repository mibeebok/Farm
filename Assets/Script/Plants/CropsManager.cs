using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CropsManager : MonoBehaviour
{
    [Header("Tilemaps")]
    [SerializeField] private Tilemap groundTilemap;
    [SerializeField] private Tilemap cropTilemap;
    
    [Header("Ground Tiles")]
    [SerializeField] private TileBase plowed;
    [SerializeField] private TileBase watered;
    [SerializeField] private TileBase toWater;
    
    [Header("Crop Prefabs")]
    [SerializeField] private Crop potatoPrefab;
    [SerializeField] private Crop carrotPrefab;
    [SerializeField] private Crop beetrootPrefab;
    [SerializeField] private Crop raspberryPrefab;
    
    private Dictionary<Vector3Int, Crop> allCrops = new Dictionary<Vector3Int, Crop>();

    // Остальные методы остаются без изменений
    
    public void CollectCrop(Vector3Int position)
    {
        if (allCrops.TryGetValue(position, out Crop crop))
        {
            // Добавляем урожай в инвентарь
            if (InventoryController.Instance != null && crop != null)
            {
                Item harvestItem = crop.GetHarvestItem();
                if (harvestItem != null)
                {
                    InventoryController.Instance.AddItem(harvestItem, 1);
                }
            }
            
            allCrops.Remove(position);
            cropTilemap.SetTile(position, null);
            groundTilemap.SetTile(position, plowed);
        }
    }
}