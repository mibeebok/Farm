// using UnityEngine;
// using System.IO;
// using System.Collections.Generic;

// public static class SaveSystem
// {
//     private static string SavePath => Path.Combine(Application.persistentDataPath, "farm_save.json");

//     public static void SaveCellState(Vector2Int gridPos, bool isPlowed, bool isWatered, int cropType, float growthStage)
//     {
//         var saveData = LoadSaveData();
//         var cellData = saveData.cellData.Find(c => c.gridPosition == gridPos);

//         if (cellData == null)
//         {
//             cellData = new SoilCellData { gridPosition = gridPos };
//             saveData.cellData.Add(cellData);
//         }

//         cellData.isPlowed = isPlowed;
//         cellData.isWatered = isWatered;
//         cellData.cropType = cropType;
//         cellData.growthStage = growthStage;

//         SaveGame(saveData);
//     }

//     public static SoilCellData LoadCellState(Vector2Int gridPos)
//     {
//         var saveData = LoadSaveData();
//         return saveData.cellData.Find(c => c.gridPosition == gridPos);
//     }

//     private static void SaveGame(FarmSaveData data)
//     {
//         string json = JsonUtility.ToJson(data, prettyPrint: true);
//         File.WriteAllText(SavePath, json);
//         Debug.Log($"Game saved to {SavePath}");
//     }

//     private static FarmSaveData LoadSaveData()
//     {
//         if (File.Exists(SavePath))
//         {
//             string json = File.ReadAllText(SavePath);
//             return JsonUtility.FromJson<FarmSaveData>(json);
//         }
//         return new FarmSaveData();
//     }

//     public static void ClearSaveData()
//     {
//         if (File.Exists(SavePath))
//             File.Delete(SavePath);
//     }
// }
