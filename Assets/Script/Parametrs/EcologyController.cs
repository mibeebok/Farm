using UnityEngine;
using UnityEngine.UI;

public class EcologyController : MonoBehaviour
{
    [Header("Sleep Setting")]
    [SerializeField] private Slider ecoSlider;
    [SerializeField] private float maxEco = 150f;
    [SerializeField] private Image fillImage;

    [Header("Color Settings")]
    [SerializeField] private Color wellColor = Color.green;
    [SerializeField] private Color sosoColor = new Color(1f, 0.5f, 0f);
    [SerializeField] private Color badColor = Color.red;

    public static float CurrentEco { get; private set; }

    private void Start()
    {
        CurrentEco = maxEco;

        if (ecoSlider == null)
            ecoSlider = GetComponent<Slider>();
        if (fillImage == null && ecoSlider != null)
            fillImage = ecoSlider.fillRect.GetComponent<Image>();
    }
    void Update()
    {
        UpdateEcoUI();
    }

    private void UpdateEcoUI()
    {
        if (ecoSlider != null)
        {
            ecoSlider.value = CurrentEco;
            if (fillImage != null)
            {
                fillImage.color = CurrentEco switch
                {
                    >= 50f => wellColor,
                    >= 30f => sosoColor,
                    _ => badColor
                };
            }
        }
    }

    public static void AddEco(float amount)
    {
        CurrentEco = Mathf.Clamp(CurrentEco + amount, 0f, 150f);
    }

    public static void RecudeEco(float amount)
    {
        CurrentEco = Mathf.Clamp(CurrentEco - amount, 0f, 150f);
    }
}
