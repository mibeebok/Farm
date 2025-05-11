using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WindowExitController : MonoBehaviour
{
    [Header("Кнопки выбора")]
    public Button yesButton;
    public Button noButton;

    [Header("Настройки")]
    [SerializeField] private float exitDelay = 0.5f;
    [SerializeField] private GameObject menuParent;

    private void OnEnable()
    {
        if (menuParent != null)
        {
            SetMenuInteractable(false);
        }
    }

    private void Start()
    {
        // Проверка и инициализация кнопок
        if (yesButton == null || noButton == null)
        {
            Debug.LogError("Кнопки не назначены в инспекторе!");
            return;
        }

        yesButton.onClick.RemoveAllListeners();
        yesButton.onClick.AddListener(OnYesClicked);

        noButton.onClick.RemoveAllListeners();
        noButton.onClick.AddListener(OnNoClicked);
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
        Debug.Log("Начало процесса выхода из игры");
        CloseWindow();
        
        yield return new WaitForSeconds(exitDelay);
        
        Debug.Log("Завершение игры");
        QuitGame();
    }

    private void QuitGame()
    {
        #if UNITY_EDITOR
        Debug.Log("Выход из Play Mode (редактор)");
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Debug.Log("Выход из приложения");
        Application.Quit();
        
        // Принудительный выход для некоторых платформ
        System.Diagnostics.Process.GetCurrentProcess().Kill();
        #endif
    }

    private void CloseWindow()
    {
        Debug.Log("Закрытие окна выхода");
        if (menuParent != null)
        {
            SetMenuInteractable(true);
        }
        gameObject.SetActive(false);
    }

    private void SetMenuInteractable(bool state)
    {
        Debug.Log(state ? "Разблокировка меню" : "Блокировка меню");
        foreach (var btn in menuParent.GetComponentsInChildren<Button>(true))
        {
            btn.interactable = state;
        }
    }
}