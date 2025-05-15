using UnityEngine;
using System.Collections;

public class SoilTile : MonoBehaviour
{
    public Sprite normalSprite;
    public Sprite plowedSprite;
    public Sprite wateredSprite;

    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
            Debug.LogError("SpriteRenderer не найден на " + gameObject.name);
    }

    public void Plow()
    {
        if (spriteRenderer != null && plowedSprite != null)
        {
            Debug.Log("Меняем спрайт на вспаханную землю.");
            spriteRenderer.sprite = plowedSprite;
        }
        else
        {
            Debug.LogWarning("Не удалось вспахать: spriteRenderer или plowedSprite отсутствует.");
        }
    }

    public IEnumerator WaterWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Water();
    }

    public void Water()
    {
        if (spriteRenderer != null && wateredSprite != null)
        {
            Debug.Log("Полив земли — меняем спрайт.");
            spriteRenderer.sprite = wateredSprite;
        }
    }
}
