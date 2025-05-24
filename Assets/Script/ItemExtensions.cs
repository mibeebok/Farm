using UnityEngine;

public static class ItemExtensions
{
    public static bool IsSeed(this Item item) => item?.type == ItemType.Seed;
    public static bool IsVegetable(this Item item) => item?.type == ItemType.Vegetable;

    public static CropType GetCropType(this Item item)
    {
        if (item == null) return CropType.None;
        return item.cropType; // Теперь тип растения хранится прямо в предмете
    }
}