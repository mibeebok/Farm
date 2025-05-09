using UnityEngine;

using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    [Header("Кнопки меню")]
    public GameObject buttonContinue;
    public GameObject buttonSave;
    public GameObject buttonExit;
    public GameObject buttonSetting;

    [Header("Ссылки")]
    public GameObject menuPanel;
    public PauseButtonPosition pauseButtonController; // исправлено на lowercase

    void Start()
    {
        // Проверка меню
        if (menuPanel == null)
        {
            Debug.LogError("Не назначен объект меню (menuPanel)!");
        }

        // Автопоиск контроллера паузы
        if (pauseButtonController == null)
        {
            pauseButtonController = FindObjectOfType<PauseButtonPosition>();
            if (pauseButtonController == null)
            {
                Debug.LogError("PauseButtonPosition не найден на сцене!");
            }
            else
            {
                Debug.Log("PauseButtonPosition найден автоматически");
            }
        }

        if (buttonContinue != null)
        {
            // Явное указание пространства имён
            UnityEngine.UI.Button btn = buttonContinue.GetComponent<UnityEngine.UI.Button>();
            if (btn != null)
            {
                btn.onClick.AddListener(Continue);
            }
            else
            {
                Debug.LogError("Компонент Button не найден на buttonContinue!");
            }
        }
    }

    public void Continue()
    {
        // Закрываем меню
        if (menuPanel != null)
        {
            menuPanel.SetActive(false);
            Debug.Log("Меню закрыто");
        }

        // Возобновляем игру
        if (pauseButtonController != null)
        {
            pauseButtonController.ResumeGame();
        }
        else
        {
            Debug.LogWarning("Контроллер паузы не найден, использую Time.timeScale");
            Time.timeScale = 1f;
        }
    }

    public void Save() {

    }

    public void Exit() {

    }

    public void Setting(){

    }
}