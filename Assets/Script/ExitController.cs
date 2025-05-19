using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
[RequireComponent(typeof(Button))]
public class ExitController : MonoBehaviour
{
    [Header("Основные настройки")]
    [SerializeField] private GameObject exitWindow;
    [SerializeField] private Button yesButton;
    [SerializeField] private Button noButton;
    [SerializeField] private float clickCooldown = 0.5f;

    private float _lastClickTime;
    private Button _button;

    private void Awake()
    {
        // Автоматическая настройка ссылок
        _button = GetComponent<Button>();
        
        if (exitWindow == null)
            Debug.LogError("ExitWindow не назначен!", this);
    }

    private void Start()
    {
        // Инициализация окна
        if (exitWindow != null)
        {
            exitWindow.SetActive(false);
            Debug.Log("Окно выхода инициализировано", exitWindow);
        }

        // Настройка кнопок
        _button.onClick.AddListener(OnExitButtonClick);
        
        if (yesButton != null)
            yesButton.onClick.AddListener(OnYesClicked);
        else
            Debug.LogError("YesButton не назначен!", this);
        
        if (noButton != null)
            noButton.onClick.AddListener(OnNoClicked);
        else
            Debug.LogError("NoButton не назначен!", this);
    }

    private void OnExitButtonClick()
    {
        if (Time.unscaledTime - _lastClickTime < clickCooldown) return;
        _lastClickTime = Time.unscaledTime;
        
        ToggleWindow();
    }

    private void ToggleWindow()
    {
        if (exitWindow == null) return;
        
        bool newState = !exitWindow.activeSelf;
        exitWindow.SetActive(newState);
        
        Debug.Log(newState ? "Окно открыто" : "Окно закрыто");
    }

    private void OnYesClicked()
    {
        Debug.Log("Выход из приложения");
        Application.Quit();
        
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }

    private void OnNoClicked()
    {
        if (exitWindow != null)
            exitWindow.SetActive(false);
    }

    private void OnDestroy()
    {
        // Чистка событий
        _button.onClick.RemoveListener(OnExitButtonClick);
        
        if (yesButton != null)
            yesButton.onClick.RemoveListener(OnYesClicked);
        
        if (noButton != null)
            noButton.onClick.RemoveListener(OnNoClicked);
    }
}