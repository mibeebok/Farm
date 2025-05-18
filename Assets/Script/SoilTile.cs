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
}
