using UnityEngine;
using System.Collections;


public class MattockController : Sounds
{
    [Header("References")]
    public InventoryController inventoryController;
    public Animator handsAnimator;

    [Header("Mattock Setting")]
    public int mattockItemId = 2;
    public string mattockBool = "Mattock";
    public float soilMattockDelay = 0.4f;

    [Header("Sprites")]
    public Sprite plowedSprite; // Спрайт для вспаханной земли

    private bool isMattock = false;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            TryMattockSoil();
        }
    }

    void TryMattockSoil()
    {
        if (inventoryController == null || inventoryController.GetSelectedSlot() != 1) 
            return;

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

        if (hit.collider != null)
        {
            // Ищем SoilTile в самом объекте или его родителях
            SoilTile soilTile = hit.collider.GetComponent<SoilTile>() ?? 
                            hit.collider.GetComponentInParent<SoilTile>();

            if (soilTile != null)
            {
                Debug.Log("Найдена земля для вспашки");
                
                // Запускаем анимацию
                if (handsAnimator != null && !isMattock)
                {
                    PlaySound(sounds[0],volume: 0.3f, p1:0.9f, p2: 1.2f);
                    handsAnimator.SetBool(mattockBool, true);
                    StartCoroutine(ResetMattockBool());
                }

                // Взаимодействуем через метод Plow()
                soilTile.Plow();
                SaveSystem.SaveAllTiles();
            }
        }
    }
    IEnumerator ResetMattockBool()
    {
        isMattock = true;

        AnimationClip mattockClip = GetAnimationClipByName("Mattock");
        float duration = mattockClip != null ? mattockClip.length : 1f;

        yield return new WaitForSeconds(duration);

        handsAnimator.SetBool(mattockBool, false);
        isMattock = false;
    }

    AnimationClip GetAnimationClipByName(string name)
    {
        RuntimeAnimatorController ac = handsAnimator.runtimeAnimatorController;
        foreach (var clip in ac.animationClips)
        {
            if (clip.name == name)
                return clip;
        }
        Debug.LogWarning("Анимация " + name + " не найдена!");
        return null;
    }
}