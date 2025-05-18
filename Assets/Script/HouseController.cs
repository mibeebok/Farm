using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;

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
    [SerializeField] private float sleepRestoreAmount = 100f; // Полное восстановление сна
    [SerializeField] private float sleepDepletionRate = 1.5f; // Множитель расхода сна (1.5 = на 50% быстрее)

    private Transform player;
    private bool isInteractable = true;
    private const string DaysPassedKey = "DaysPassed";
    private SleepController sleepController;
    public static event Action OnNewDay;

    public static int DaysPassed
    {
        get => PlayerPrefs.GetInt(DaysPassedKey, 0);
        private set => PlayerPrefs.SetInt(DaysPassedKey, value);
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        sleepController = FindObjectOfType<SleepController>();
        
        if (!playerVisual && player)
            playerVisual = player.Find("PlayerVisual");

        if (!doorAnimator)
            Debug.LogError("Animator не назначен!");

        if (darknessPanel)
        {
            darknessPanel.alpha = 0;
            darknessPanel.blocksRaycasts = false;
        }
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

        // Открываем дверь
        if (doorAnimator)
        {
            doorAnimator.SetBool(doorBoolParameter, true);
            yield return new WaitForSeconds(0.5f);
        }

        // Скрываем персонажа
        if (playerVisual)
            playerVisual.gameObject.SetActive(false);
        else
            Debug.LogError("PlayerVisual не найден");

        // Затемнение экрана
        yield return StartCoroutine(FadeScreen(0f, 1f));
        
        // Восстанавливаем сон
        if (sleepController != null)
        {
            sleepController.RestoreSleep(sleepRestoreAmount);
        }
        
        // Увеличиваем скорость расхода сна
        if (sleepController != null)
        {
            sleepController.SetDepletionRate(sleepDepletionRate);
        }

        // Ждем ночь
        yield return new WaitForSeconds(nightDuration);
        
        // Осветление экрана
        yield return StartCoroutine(FadeScreen(1f, 0f));

        // Показываем персонажа
        if (playerVisual) 
            playerVisual.gameObject.SetActive(true);

        // Закрываем дверь
        if (doorAnimator)
            doorAnimator.SetBool(doorBoolParameter, false);

         // Увеличиваем счетчик дней
        DaysPassed++;
        Debug.Log($"Всего дней: {DaysPassed}");
        
        // Уведомляем о новом дне
        OnNewDay?.Invoke();

        isInteractable = true;
    }

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