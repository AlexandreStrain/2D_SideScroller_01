using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatBarUI : MonoBehaviour
{
    [SerializeField] private TMP_Text info;
    [SerializeField] private Slider uiElement;

    public void Init(float max)
    {
        SetInitialSliderValues(max);
        SetText();
    }

    public void Set(float amount)
    {
        SetSliderValue(amount);
        SetText();
    }

    private void SetText()
    {
        info.text = $"{uiElement.value}/{uiElement.maxValue}";
    }
    private void SetInitialSliderValues(float max)
    {
        uiElement.maxValue = max;
        uiElement.value = max;
    }

    private void SetSliderValue(float amount)
    {
        uiElement.value = amount;
    }
}
