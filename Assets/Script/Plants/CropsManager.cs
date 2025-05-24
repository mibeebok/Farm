using System.Collections.Generic;
using UnityEngine;

public class CropsManager : MonoBehaviour
{
    public static CropsManager Instance { get; private set; }
    [Header("Crop Prefabs")]
    [SerializeField] private Potato potatoPrefab;
    [SerializeField] private Carrot carrotPrefab;
    [SerializeField] private Beetroot beetrootPrefab;
    [SerializeField] private Rastberry rastberryPrefab;
    
    private Dictionary<Vector2Int, Crop> allCrops = new Dictionary<Vector2Int, Crop>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // DontDestroyOnLoad(gameObject); // Раскомментируйте если нужно сохранять между сценами
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public bool CanPlantAt(Vector2Int gridPosition)
    {
        GameObject tileObject = FarmGrid.Instance.GetTileAt(gridPosition);
        if (tileObject == null) return false;

        var soilTile = tileObject.GetComponent<SoilTile>();
        return soilTile != null && soilTile.IsReadyForPlanting() &&
               !allCrops.ContainsKey(gridPosition);
    }
    public bool TryPlantSeed(Item seedItem, Vector2 worldPosition)
    {
        if (!seedItem.IsSeed()) 
        {
            Debug.Log("Предмет не является семенами");
            return false;
        }
        
        Vector2Int gridPos = FarmGrid.Instance.WorldToGridPosition(worldPosition);
        
        if (!CanPlantAt(gridPos))
        {
            Debug.Log($"Нельзя посадить здесь. Готова ли почва: {FarmGrid.Instance.GetTileAt(gridPos)?.GetComponent<SoilTile>()?.IsReadyForPlanting()}");
            return false;
        }

        Crop cropPrefab = GetCropPrefab(seedItem.cropType);
        if (cropPrefab == null)
        {
            Debug.Log($"Не найден префаб для {seedItem.cropType}");
            return false;
        }

        // Создаем растение
        Crop newCrop = Instantiate(cropPrefab, 
            FarmGrid.Instance.GridToWorldPosition(gridPos), 
            Quaternion.identity);
        
        // Сохраняем растение
        allCrops[gridPos] = newCrop;
        
        // Сбрасываем состояние почвы
        FarmGrid.Instance.GetTileAt(gridPos)?.GetComponent<SoilTile>()?.ResetAfterPlanting();
        
        Debug.Log($"Посажено: {seedItem.name} на позиции {gridPos}");
        return true;
    }

    public bool PlantCrop(Vector2Int gridPosition, CropType cropType)
    {
        if (!CanPlantAt(gridPosition)) return false;

        Crop cropPrefab = GetCropPrefab(cropType);
        if (cropPrefab == null) return false;

        var tileObject = FarmGrid.Instance.GetTileAt(gridPosition);
        if (tileObject == null) return false;

        // Создаем копию ScriptableObject
        Crop newCrop = Instantiate(cropPrefab);
        newCrop.transform = tileObject.transform; // Устанавливаем transform

        // Обновляем состояние земли
        tileObject.GetComponent<SoilTile>()?.ResetAfterPlanting();

        // Сохраняем растение в словарь
        allCrops[gridPosition] = newCrop;

        return true;
    }

    private Crop GetCropPrefab(CropType cropType)
    {
        switch (cropType)
        {
            case CropType.Potato: return potatoPrefab;
            case CropType.Carrot: return carrotPrefab;
            case CropType.Beetroot: return beetrootPrefab;
            case CropType.Rastberry: return rastberryPrefab;
            default: return null;
        }
    }

    public void OnNewDay()
    {
        foreach (var crop in allCrops.Values)
        {
            crop.Grow();
        }
        
        // Сбрасываем полив для всех тайлов
        FindObjectOfType<SoilTileWateringCan>()?.ResetAllWateredTiles();
    }

    public void CollectCrop(Vector2Int gridPosition)
    {
        if (allCrops.TryGetValue(gridPosition, out Crop crop))
        {
            Item harvestItem = crop.GetHarvestItem();
            if (harvestItem != null && InventoryController.Instance != null)
            {
                InventoryController.Instance.AddItem(harvestItem, 1);
            }
            
            allCrops.Remove(gridPosition);
        }
    }
}