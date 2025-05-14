using UnityEngine;

public class FarmLand : MonoBehaviour
{
    [SerializeField] private Sprite drySprite;
    [SerializeField] private Sprite wateredSprite;
    
    private SpriteRenderer spriteRenderer;
    private bool isWatered = false;
    
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        SetDry();
    }
    
    public void Water()
    {
        if (!isWatered)
        {
            isWatered = true;
            spriteRenderer.sprite = wateredSprite;
            // Дополнительная логика при поливе
        }
    }
    
    public void SetDry()
    {
        isWatered = false;
        spriteRenderer.sprite = drySprite;
    }
}