using UnityEngine;
using System.Collections;

public class HouseController : MonoBehaviour
{
    [Header("Настройки")]
    [SerializeField] private float interactionDistance = 3f;
    [SerializeField] private Animator doorAnimator;
    [SerializeField] private string doorBoolParameter = "IsOpen";
    [SerializeField] private float nightDuration = 6f;
    [SerializeField] private CanvasGroup darknessPanel;
    [SerializeField] private float fadeDuration = 1f;
    [SerializeField] private Transform playerVisual;

    private Transform player;
    private bool isInteractable = true;
    private const string DaysPassedKey = "DaysPassed"; // Ключ для сохранения

    public static int DaysPassed
    {
        get => PlayerPrefs.GetInt(DaysPassedKey, 0);
        private set => PlayerPrefs.SetInt(DaysPassedKey, value);
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        
        if (!playerVisual && player)
            playerVisual = player.Find("PlayerVisual");

        if (!doorAnimator)
            Debug.LogError("Animator не назначен!");

        if (darknessPanel)
        {
            darknessPanel.alpha = 0;
            darknessPanel.blocksRaycasts = false;
        }

        // Загрузка сохраненного количества дней при старте
        Debug.Log($"Загружено дней: {DaysPassed}");
    }

    private void Update()
    {
        if (!isInteractable || !player) return;

        if (Vector3.Distance(transform.position, player.position) <= interactionDistance)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                StartCoroutine(NightTransition());
            }
        }
    }

    private IEnumerator NightTransition()
    {
        isInteractable = false;

        if (doorAnimator)
        {
            doorAnimator.SetBool(doorBoolParameter, true);
            yield return new WaitForSeconds(0.5f);
        }

        if (playerVisual)
        {
            playerVisual.gameObject.SetActive(false);
        }
        else Debug.LogError("PlayerVisual не найден");

        yield return StartCoroutine(FadeScreen(0f, 1f));
        yield return new WaitForSeconds(nightDuration);
        yield return StartCoroutine(FadeScreen(1f, 0f));

        if (playerVisual) 
            playerVisual.gameObject.SetActive(true);

        if (doorAnimator)
            doorAnimator.SetBool(doorBoolParameter, false);

        // Увеличиваем и сохраняем количество дней
        DaysPassed++;
        Debug.Log($"Всего дней: {DaysPassed}");

        isInteractable = true;
    }

    // Остальные методы без изменений
    private IEnumerator FadeScreen(float start, float end)
    {
        if (!darknessPanel) yield break;
        
        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            darknessPanel.alpha = Mathf.Lerp(start, end, elapsed/fadeDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        darknessPanel.alpha = end;
        darknessPanel.blocksRaycasts = end > 0.5f;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0, 0.5f, 1f, 0.3f);
        Gizmos.DrawSphere(transform.position, interactionDistance);
    }
}