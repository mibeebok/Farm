using UnityEngine;
using System.Collections;

public class WateringCanController : Sounds
{
    [Header("References")]
    public InventoryController inventoryController;
    public Animator handsAnimator;

    [Header("Watering Settings")]
    public int wateringCanItemId = 1; // ID лейки
    public string wateringBool = "Water"; // Имя параметра bool
    public float soilWateringDelay = 0.4f;

    private bool isWatering = false;

    void Update()
    {
        // Проверяем, выбран ли нужный слот и нажата ЛКМ
        if (Input.GetMouseButtonDown(0))
        {
            TryWaterSoil();
        }
    }

    void TryWaterSoil()
    {
        if (inventoryController == null) return;
        if (inventoryController.GetSelectedSlot() != 0) return;

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

        if (hit.collider != null)
        {
            SoilTile soilTile = hit.collider.GetComponent<SoilTile>();
            if (soilTile != null && soilTile.isPlowed)
            {
                PlaySound(sounds[0], volume: 0.3f, p1: 0.9f, p2: 1.2f);
                
                if (handsAnimator != null && !isWatering)
                {
                    handsAnimator.SetBool(wateringBool, true);
                    StartCoroutine(ResetWateringBool());
                }

                SoilTileWateringCan soilWater = hit.collider.GetComponent<SoilTileWateringCan>();
                if (soilWater != null)
                {
                    StartCoroutine(DelayedWatering(soilWater, soilTile));
                }
            }
        }
    }

    IEnumerator DelayedWatering(SoilTileWateringCan soilWater, SoilTile soilTile)
    {
        yield return new WaitForSeconds(soilWateringDelay);
        soilWater.Water();
        soilTile.UpdateSoilSprite(); // Явное обновление спрайта
        SaveSystem.SaveAllTiles();
    }

    IEnumerator ResetWateringBool()
    {
        isWatering = true;

        AnimationClip wateringClip = GetAnimationClipByName("Watering");
        float duration = wateringClip != null ? wateringClip.length : 1f;

        yield return new WaitForSeconds(duration);

        handsAnimator.SetBool(wateringBool, false);
        isWatering = false;
    }

    AnimationClip GetAnimationClipByName(string name)
    {
        RuntimeAnimatorController ac = handsAnimator.runtimeAnimatorController;
        foreach (var clip in ac.animationClips)
        {
            if (clip.name == name)
                return clip;
        }
        return null;
    }
}
