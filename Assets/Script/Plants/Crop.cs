using UnityEngine;
using UnityEngine.Tilemaps;

// Базовый класс для всех сельхоз культур
public abstract class Crop : ScriptableObject
{
    public CropType cropType;
    public bool isSeed; // True для семян, false для овощей
    
    [Header("Base Settings")]
    public Sprite icon;
    public int maxStack = 32;
    
    [Header("Growth Settings")] 
    public Vector3Int position;
    public float timeRemaining;
    public bool timerIsRunning = false;
    public int currentGrowthStage = 0;
    public TileBase[] growthStages = new TileBase[3]; // stage0, stage1, stage2
    
    [Header("Harvest Settings")]
    public Item harvestItem;
    public int harvestYield = 1; // Количество получаемого урожая

    public Transform transform { get; set; }

    public virtual void Initialize(Vector3Int pos, float growthTime)
    {
        position = pos;
        timeRemaining = growthTime;
        currentGrowthStage = 0;
        timerIsRunning = true;
    }

    public virtual TileBase GetCurrentTile()
    {
        if (currentGrowthStage >= 0 && currentGrowthStage < growthStages.Length)
            return growthStages[currentGrowthStage];
        return growthStages[0];
    }

    public virtual void Grow()
    {
        if (currentGrowthStage < growthStages.Length - 1)
        {
            currentGrowthStage++;
            OnGrowthStageChanged();
        }
    }

    protected virtual void OnGrowthStageChanged()
    {
        // Можно переопределить в дочерних классах
    }

    public virtual Item GetHarvestItem()
    {
        return harvestItem;
    }

    public virtual int GetHarvestYield()
    {
        return harvestYield;
    }
}

// Класс для семян
[CreateAssetMenu(fileName = "NewSeed", menuName = "Crops/Seed")]
public class Seed : Crop
{
    [Header("Seed Specific")]
    public float growthTime = 120f;
    public float waterRequirement = 1f;

    private void Awake()
    {
        isSeed = true;
        maxStack = 32; // Семена можно складывать больше
    }
}

// Класс для овощей
[CreateAssetMenu(fileName = "NewVegetable", menuName = "Crops/Vegetable")]
public class Vegetable : Crop
{
    [Header("Vegetable Specific")]
    public int sellPrice = 10;
    public float nutritionValue = 1f;

    private void Awake()
    {
        isSeed = false;
        maxStack = 16; // Овощи меньше стэкаются
    }
}