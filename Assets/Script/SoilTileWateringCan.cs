using System.Collections;
using UnityEngine;

public class SoilTileWateringCan : MonoBehaviour
{
    public bool isWatered = false;
    public Color dryColor = Color.white;
    public Color wateredColor = new Color(0.5f, 0.8f, 1f);
    public SpriteRenderer sr;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        UpdateVisual();
    }

    public void Water()
    {
        if (isWatered) return;
        isWatered = true;
        UpdateVisual();
    }

    public IEnumerator WaterWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Water();
    }

    public void UpdateVisual()
    {
        sr.color = isWatered ? wateredColor : dryColor;
    }
    public void SetWateredState(bool watered)
    {
        isWatered = watered;
        UpdateVisual();
    }
    public void ResetAllWateredTiles()
    {
        SoilTileWateringCan[] wateredTiles = FindObjectsOfType<SoilTileWateringCan>();
        foreach (SoilTileWateringCan tile in wateredTiles)
        {
            tile.SetWateredState(false);
        }
        Debug.Log($"Сброшено {wateredTiles.Length} политых блоков");
    }
    
}