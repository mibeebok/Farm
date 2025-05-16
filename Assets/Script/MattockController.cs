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
        if (inventoryController == null) return;
        if (inventoryController.GetSelectedSlot() != 1) return;
        Debug.Log("Выбран слот: " + inventoryController.GetSelectedSlot());

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

        if (hit.collider != null)
        {
            Transform tileTransform = hit.collider.transform;

            // Проверка тега у родителя (FarmGrid > Tile)
            if (tileTransform.CompareTag("Soil") || tileTransform.parent.CompareTag("Soil"))
            {
                Debug.Log("Клик леврй кнопкой по земле (мотыга)!");

                if (handsAnimator != null && !isMattock)
                {
                    Debug.Log("Анимация мотыги запускается.");
                    handsAnimator.SetBool(mattockBool, true);
                    StartCoroutine(ResetMattockBool());
                }

                SpriteRenderer renderer = tileTransform.GetComponent<SpriteRenderer>();
                if (renderer != null && plowedSprite != null)
                {
                    Debug.Log("Меняем спрайт на вспаханный.");
                    renderer.sprite = plowedSprite;
                }
                else
                {
                    Debug.LogWarning("SpriteRenderer или plowedSprite не найден.");
                }
                
            }
            else
            {
                Debug.Log("Объект не имеет тег Soil.");
            }
        }
        else
        {
            Debug.Log("Ничего не попало под клик.");
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
