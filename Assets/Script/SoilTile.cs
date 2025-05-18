using UnityEngine;
using System.Collections;

public class SoilTile : MonoBehaviour
{
    [Header("Sprites")]
    public Sprite normalSprite;
    public Sprite plowedSprite;
    public Sprite wateredSprite;

    private SpriteRenderer spriteRenderer;
    private bool isPlowed = false;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>() ?? gameObject.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = normalSprite;
    }

    public void LoadFromSaveData(SaveData data)
    {
        if (data == null || spriteRenderer == null) return;

        isPlowed = data.isPlowed;
        spriteRenderer.sprite = isPlowed ? 
            (data.isWatered ? wateredSprite : plowedSprite) : 
            normalSprite;
    }

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        }

        // Инициализация начального спрайта
        spriteRenderer.sprite = normalSprite;
    }

    public void Plow()
    {
        if (!isPlowed && plowedSprite != null)
        {
            spriteRenderer.sprite = plowedSprite;
            isPlowed = true;
            Debug.Log("Земля вспахана!");
        }
    }
    public bool IsPlowed => isPlowed;

    public SaveData GetSaveData()
    {
        return new SaveData
        {
            position = transform.position,
            isPlowed = this.isPlowed,
            isWatered = GetComponent<SoilTileWateringCan>()?.isWatered ?? false
        };
    }

    // public void LoadFromSaveData(SaveData data)
    // {
    //     if (data == null) return;
        
    //     isPlowed = data.isPlowed;
    //     if (isPlowed && plowedSprite != null)
    //     {
    //         spriteRenderer.sprite = plowedSprite;
    //     }

    //     var wateringCan = GetComponent<SoilTileWateringCan>();
    //     if (wateringCan != null)
    //     {
    //         wateringCan.isWatered = data.isWatered;
    //         wateringCan.UpdateVisual();
    //     }
    // }
}
