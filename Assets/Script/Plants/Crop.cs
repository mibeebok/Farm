using UnityEngine;
using UnityEngine.Tilemaps;

public abstract class Crop : ScriptableObject
{
    public Vector3Int position;
    public float timeRemaining;
    public bool timerIsRunning = false;
    public int currentGrowthStage = 0;

    [Header("Growth Tiles")]
    public TileBase stage0;
    public TileBase stage1;
    public TileBase stage2;

    public virtual void Initialize(Vector3Int pos, float growthTime)
    {
        position = pos;
        timeRemaining = growthTime;
    }

    public virtual TileBase GetCurrentState()
    {
        return currentGrowthStage switch
        {
            0 => stage0,
            1 => stage1,
            2 => stage2,
            _ => stage0
        };
    }

    public virtual bool AdvanceToNextStage()
    {
        if (currentGrowthStage < 2) // 3 стадии (0,1,2)
        {
            currentGrowthStage++;
            return true;
        }
        return false;
    }

    public virtual void ResetTimer(float newTime)
    {
        timeRemaining = newTime;
        timerIsRunning = false;
    }
    public virtual Item GetHarvestItem()
    {
        return null; // или базовый предмет урожая
    }
}