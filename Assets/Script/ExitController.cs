using UnityEngine;
using UnityEngine.EventSystems;

public class ExitController : MonoBehaviour, IPointerDownHandler
{
    [Header("Настройки")]
    public GameObject exitWindow;
    public Camera uiCamera;

    void Start()
    {
        if (exitWindow != null)
        {
            exitWindow.SetActive(false);
            Debug.Log("Окно выхода деактивировано");
        }
        else
        {
            Debug.LogError("Не назначено окно выхода!");
        }

        if (uiCamera == null)
        {
            uiCamera = Camera.main;
        }
    }

    // Реализация интерфейса для UI кликов
    public void OnPointerDown(PointerEventData eventData)
    {
        ToggleExitWindow();
    }

    private void ToggleExitWindow()
    {
        if (exitWindow != null)
        {
            bool shouldOpen = !exitWindow.activeSelf;
            exitWindow.SetActive(shouldOpen);

            if (shouldOpen)
            {
                // Центрирование окна
                Vector3 centerPos = uiCamera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 10f));
                centerPos.z = 0;
                exitWindow.transform.position = centerPos;
                
                Debug.Log("Окно выхода открыто");
            }
            else
            {
                Debug.Log("Окно выхода закрыто");
            }
        }
        else
        {
            Debug.LogError("Окно выхода не назначено!");
        }
    }
}