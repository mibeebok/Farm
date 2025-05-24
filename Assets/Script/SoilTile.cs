using UnityEngine;
using System.Collections;

public class SoilTile : MonoBehaviour
{
    [Header("Sprites")]
    public Sprite normalSprite;
    public Sprite plowedSprite;
    public Sprite wateredSprite;

    public SpriteRenderer spriteRenderer;
    public bool isPlowed = false;

    private SoilTileWateringCan wateringCan;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F5))
        {
            Debug.Log($"Состояние: Вспахана={isPlowed}, Полита={GetComponent<SoilTileWateringCan>()?.isWatered}");
            Debug.Log($"Текущий спрайт: {spriteRenderer.sprite?.name}");
        }
    }

    public void UpdateSoilSprite()
    {
        var wateringCan = GetComponent<SoilTileWateringCan>();
        if (wateringCan == null) return;

        if (wateringCan.isWatered && wateredSprite != null)
        {
            spriteRenderer.sprite = wateredSprite;
        }
        else if (isPlowed && plowedSprite != null)
        {
            spriteRenderer.sprite = plowedSprite;
        }
        else
        {
            spriteRenderer.sprite = normalSprite;
        }
    }

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>() ?? gameObject.AddComponent<SpriteRenderer>();
        wateringCan = GetComponent<SoilTileWateringCan>();
        spriteRenderer.sprite = normalSprite;
    }
    public bool IsReadyForPlanting()
    {
        var wateringCan = GetComponent<SoilTileWateringCan>();
        return isPlowed && (wateringCan != null && wateringCan.isWatered);
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
    public void Water()
    {
        if (isPlowed)
        {
            var wateringCan = GetComponent<SoilTileWateringCan>();
            if (wateringCan != null)
            {
                wateringCan.Water();
                // Не устанавливаем спрайт здесь - это делает SoilTileWateringCan
            }
        }
    }

    public void ResetAfterPlanting()
    {
        if (wateringCan != null)
        {
            wateringCan.SetWateredState(false);
        }
        spriteRenderer.sprite = plowedSprite;
    }

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
