using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
[RequireComponent(typeof(Button))]
public class ExitController : MonoBehaviour
{
    [Header("Основные настройки")]
    [Tooltip("Окно подтверждения выхода")]
    [SerializeField] private GameObject exitWindow;
    
    [Tooltip("Кнопка подтверждения выхода")]
    [SerializeField] private Button confirmButton;
    
    [Tooltip("Кнопка отмены выхода")]
    [SerializeField] private Button cancelButton;
    
    [Tooltip("Задержка между кликами (секунды)")]
    [SerializeField] private float clickCooldown = 0.5f;

    private Button _exitButton;
    private float _lastClickTime;
    private Canvas _parentCanvas;

    private void Awake()
    {
        // Инициализация компонентов
        _exitButton = GetComponent<Button>();
        
        #if UNITY_EDITOR
        // Проверка ссылок в редакторе
        if (exitWindow == null)
            Debug.LogError("Не назначено окно выхода!", this);
        if (confirmButton == null)
            Debug.LogError("Не назначена кнопка подтверждения!", this);
        if (cancelButton == null)
            Debug.LogError("Не назначена кнопка отмены!", this);
        #endif
    }

    private void Start()
    {
        // Находим родительский Canvas
        _parentCanvas = exitWindow.GetComponentInParent<Canvas>();
        
        // Инициализация окна
        if (exitWindow != null)
        {
            exitWindow.SetActive(false);
            CenterWindow();
        }

        // Подписка на события кнопок
        _exitButton.onClick.AddListener(OnExitButtonClick);
        
        if (confirmButton != null)
            confirmButton.onClick.AddListener(OnConfirmClick);
        
        if (cancelButton != null)
            cancelButton.onClick.AddListener(OnCancelClick);
    }

    private void OnExitButtonClick()
    {
        // Защита от спама кликами
        if (Time.unscaledTime - _lastClickTime < clickCooldown) return;
        _lastClickTime = Time.unscaledTime;
        
        ToggleExitWindow();
    }

    private void ToggleExitWindow()
    {
        if (exitWindow == null) return;
        
        bool shouldOpen = !exitWindow.activeSelf;
        exitWindow.SetActive(shouldOpen);
        
        if (shouldOpen)
        {
            CenterWindow();
            Debug.Log("Окно выхода открыто", exitWindow);
        }
        else
        {
            Debug.Log("Окно выхода закрыто");
        }
    }

    private void CenterWindow()
    {
        if (exitWindow == null) return;

        // Для UI элементов
        if (exitWindow.TryGetComponent<RectTransform>(out var rectTransform))
        {
            rectTransform.anchorMin = Vector2.one * 0.5f;
            rectTransform.anchorMax = Vector2.one * 0.5f;
            rectTransform.pivot = Vector2.one * 0.5f;
            rectTransform.anchoredPosition = Vector2.zero;
        }
        // Для обычных GameObject
        else
        {
            exitWindow.transform.position = GetScreenCenter();
        }
    }

    private Vector3 GetScreenCenter()
    {
        if (Camera.main != null)
            return Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 10f));
        
        return new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0f);
    }

    private void OnConfirmClick()
    {
        Debug.Log("Подтверждение выхода");
        
        // Выход из приложения
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }

    private void OnCancelClick()
    {
        Debug.Log("Отмена выхода");
        ToggleExitWindow();
    }

    private void OnDestroy()
    {
        // Отписка от событий для предотвращения утечек памяти
        if (_exitButton != null)
            _exitButton.onClick.RemoveListener(OnExitButtonClick);
        
        if (confirmButton != null)
            confirmButton.onClick.RemoveListener(OnConfirmClick);
        
        if (cancelButton != null)
            cancelButton.onClick.RemoveListener(OnCancelClick);
    }
}