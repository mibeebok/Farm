using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WindowExitController : MonoBehaviour
{
    [Header("Кнопки выбора")]
    [SerializeField] private Button yesButton;
    [SerializeField] private Button noButton;

    [Header("Настройки")]
    [SerializeField] private float exitDelay = 0.5f;
    [SerializeField] private GameObject menuParent;

    private void Start()
    {
        if (yesButton == null || noButton == null)
        {
            Debug.LogError("Кнопки не назначены в инспекторе!");
            enabled = false;
            return;
        }

        yesButton.onClick.AddListener(OnYesClicked);
        noButton.onClick.AddListener(OnNoClicked);
    }

    private void OnEnable()
    {
        if (menuParent != null)
        {
            SetMenuInteractable(false);
        }
    }

    private void OnYesClicked()
    {
        Debug.Log("Нажата кнопка 'Да'");
        StartCoroutine(ExitGameCoroutine());
    }

    private void OnNoClicked()
    {
        Debug.Log("Нажата кнопка 'Нет'");
        CloseWindow();
    }

    private IEnumerator ExitGameCoroutine()
{
    Debug.Log("[1] Начало корутины выхода");
    
    
    // Вариант 2: Таймер с логированием
    float timer = 0;
    while (timer < 1f)
    {
        timer += Time.deltaTime;
        Debug.Log($"Таймер: {timer}");
        yield return null;
    }
    
    Debug.Log("[3] Завершение игры");
    QuitGame();
}


private void QuitGame()
{
    #if UNITY_EDITOR
    if (UnityEditor.EditorApplication.isPlaying)
    {
        Debug.Log("Выход из Play Mode (редактор)");
        UnityEditor.EditorApplication.isPlaying = false;
        return;
    }
    #endif
    
    Debug.Log("Выход из приложения");
    Application.Quit();
    
    // Принудительный выход для платформ, где Quit() игнорируется
    #if !UNITY_EDITOR
    System.Diagnostics.Process.GetCurrentProcess().Kill();
    #endif
}

    private void CloseWindow()
    {
        Debug.Log($"Закрытие окна выхода. Текущее состояние: {gameObject.activeSelf}");
        if (menuParent != null)
        {
            SetMenuInteractable(true);
        }
        gameObject.SetActive(false);
        Debug.Log($"Окно закрыто. Новое состояние: {gameObject.activeSelf}");
    }

    private void SetMenuInteractable(bool state)
    {
        if (menuParent == null)
        {
            Debug.LogWarning("menuParent не назначен!");
            return;
        }
        
        var buttons = menuParent.GetComponentsInChildren<Button>(true);
        if (buttons == null || buttons.Length == 0)
        {
            Debug.LogWarning("Не найдены кнопки в menuParent!");
            return;
        }

        foreach (var btn in buttons)
        {
            btn.interactable = state;
        }
    }

    private void OnDestroy()
    {
        if (yesButton != null) yesButton.onClick.RemoveListener(OnYesClicked);
        if (noButton != null) noButton.onClick.RemoveListener(OnNoClicked);
    }
}