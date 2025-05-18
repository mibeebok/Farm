using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class SaveSystem
{
    private static string SavePath => Path.Combine(Application.persistentDataPath, "farm_save.json");

    public static void SaveAllTiles()
    {
        if (!FarmGrid.Instance.isGridGenerated) return;

        var saveData = new List<SaveData>();
        for (int x = 0; x < FarmGrid.Instance.gridSizeX; x++)
        {
            for (int y = 0; y < FarmGrid.Instance.gridSizeY; y++)
            {
                var tile = FarmGrid.Instance.GetTileAt(new Vector2Int(x, y));
                if (tile != null)
                {
                    var soil = tile.GetComponent<SoilTile>();
                    if (soil != null)
                    {
                        saveData.Add(soil.GetSaveData());
                    }
                }
            }
        }

        File.WriteAllText(SavePath, JsonUtility.ToJson(new Wrapper<SaveData>{ Items = saveData }, true));
    }

    public static void LoadAllTiles()
    {
        if (!FarmGrid.Instance.isGridGenerated || !File.Exists(SavePath)) 
            return;

        var loadedData = JsonUtility.FromJson<Wrapper<SaveData>>(File.ReadAllText(SavePath));
        foreach (var data in loadedData.Items)
        {
            var gridPos = FarmGrid.Instance.WorldToGridPosition(data.position);
            var tile = FarmGrid.Instance.GetTileAt(gridPos);
            tile?.GetComponent<SoilTile>()?.LoadFromSaveData(data);
        }
    }

    [System.Serializable]
    private class Wrapper<T> { public List<T> Items; }
}