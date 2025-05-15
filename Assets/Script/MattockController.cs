using UnityEngine;
using System.Collections;

public class MattockController : Sounds
{
    [Header("References")]
    public InventoryController inventoryController;
    public Animator handsAnimator;

    [Header("Mattock Setting")]
    public int mattockItemId = 2; // ID мотыги
    public string mattockBool = "Mattock";
    public float soilMattockDelay = 0.4f;

    private bool isMattock = false;

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            TryMattockSoil();
        }
    }

    void TryMattockSoil()
    {
        if (inventoryController == null) return;
        if (inventoryController.GetSelectedSlot() != 1) return;

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

        if (hit.collider != null && hit.collider.CompareTag("Soil"))
        {
            Debug.Log("Клик правой кнопкой по земле (мотыга)!");

            if (handsAnimator != null && !isMattock)
            {
                Debug.Log("Анимация мотыги запускается.");
                handsAnimator.SetBool(mattockBool, true);
                StartCoroutine(ResetMattockBool());
            }

            // Обработка земли
            SoilTile soil = hit.collider.GetComponent<SoilTile>();
            if (soil != null)
            {
                Debug.Log("Найден компонент SoilTile, запускаем вспашку.");
                soil.Plow();
            }
            else
            {
                Debug.LogWarning("Клик по земле, но SoilTile не найден.");
            }
        }
        else
        {
            Debug.Log("Правый клик — не по земле.");
        }
    }

    IEnumerator ResetMattockBool()
    {
        isMattock = true;

        // Получаем длительность анимации
        AnimationClip mattockClip = GetAnimationClipByName("Mattock"); // Название анимации!
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
