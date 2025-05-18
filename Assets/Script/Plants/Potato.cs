using UnityEngine;

[CreateAssetMenu(fileName = "Potato", menuName = "Crops/Potato", order = 1)]
public class Potato : Crop
{
    public Item harvestItem; // Добавляем предмет, который получаем при сборе
    
    public override void Initialize(Vector3Int pos, float growthTime)
    {
        base.Initialize(pos, growthTime);
        // Дополнительная инициализация для картошки
    }
}