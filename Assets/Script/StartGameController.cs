using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartGameController : MonoBehaviour
{
    [Header("Кнопки")]
    [SerializeField] private Button playButton;
    [SerializeField] private Button aboutButton;
    [SerializeField] private Button exitButton;

    [Header("Окна")]
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject aboutPanel;

    [Header("Настройки")]
    [SerializeField] private string gameSceneName = "GameScene";
    [SerializeField] private float buttonCooldown = 0.5f;

    private float _lastClickTime;

    private void Start()
    {
        // Подписка на события кнопок
        playButton.onClick.AddListener(OnPlayClick);
        aboutButton.onClick.AddListener(OnAboutClick);
        exitButton.onClick.AddListener(OnExitClick);

        // Инициализация состояний
        aboutPanel.SetActive(false);
        mainMenu.SetActive(true);
    }

    private void OnPlayClick()
    {
        if (Time.unscaledTime - _lastClickTime < buttonCooldown) return;
        _lastClickTime = Time.unscaledTime;
        
        SceneManager.LoadScene(gameSceneName);
    }

    private void OnAboutClick()
    {
        if (Time.unscaledTime - _lastClickTime < buttonCooldown) return;
        _lastClickTime = Time.unscaledTime;
        
        mainMenu.SetActive(false);
        aboutPanel.SetActive(true);
    }

    public void OnBackFromAbout()
    {
        aboutPanel.SetActive(false);
        mainMenu.SetActive(true);
    }

    private void OnExitClick()
    {
        if (Time.unscaledTime - _lastClickTime < buttonCooldown) return;
        _lastClickTime = Time.unscaledTime;
        
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }
}