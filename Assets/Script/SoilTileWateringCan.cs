using System.Collections;
using UnityEngine;

public class SoilTileWateringCan : MonoBehaviour
{
    public bool isWatered = false;
    public Color dryColor = Color.white;
    public Color wateredColor = new Color(0.5f, 0.8f, 1f);
    private SpriteRenderer sr;

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

    void UpdateVisual()
    {
        sr.color = isWatered ? wateredColor : dryColor;
    }
    
}
