using UnityEngine;
using UnityEngine.UI;

public class SleepController : MonoBehaviour
{
    [Header("Основные настройки")]
    public Slider sleepSlider;
    public float maxSleep = 100f;
    public float depletionRate = 1f; // Базовая скорость расхода
    public float depletionAmount = 0.1f; // Потеря сна в секунду
    
    [Header("Цвета состояния")]
    public Color wellRestedColor = Color.blue;
    public Color tiredColor = new Color(1f, 0.5f, 0f); // Оранжевый
    public Color exhaustedColor = Color.red;

    private float currentSleep;

    private void Start()
    {
        currentSleep = maxSleep;
        UpdateSleepUI();
    }

    private void Update()
    {
        // Расходуем сон
        currentSleep -= depletionAmount * depletionRate * Time.deltaTime;
        currentSleep = Mathf.Clamp(currentSleep, 0f, maxSleep);
        UpdateSleepUI();
    }

    public void RestoreSleep(float amount)
    {
        currentSleep = Mathf.Clamp(currentSleep + amount, 0f, maxSleep);
    }

    public void SetDepletionRate(float rate)
    {
        depletionRate = rate;
    }

    private void UpdateSleepUI()
    {
        if (sleepSlider == null) return;
        
        // Обновляем значение ползунка
        sleepSlider.value = currentSleep;
        
        // Меняем цвет ползунка в зависимости от уровня сна
        float sleepPercentage = currentSleep / maxSleep * 100f;
        
        if (sleepPercentage >= 50f)
        {
            sleepSlider.fillRect.GetComponent<Image>().color = wellRestedColor;
        }
        else if (sleepPercentage >= 30f)
        {
            sleepSlider.fillRect.GetComponent<Image>().color = tiredColor;
        }
        else
        {
            sleepSlider.fillRect.GetComponent<Image>().color = exhaustedColor;
        }
    }
}