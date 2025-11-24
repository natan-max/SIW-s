using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GeneratorUI : MonoBehaviour
{
    public Canvas canvas;
    public Slider chargeSlider;
    public TMP_Text chargeText;
    private Generator currentGenerator;

    void Start()
    {
        if (canvas != null) canvas.enabled = false;
    }

    void Update()
    {
        if (currentGenerator != null)
        {
            float value = currentGenerator.charge / 100f;
            if (chargeSlider != null) chargeSlider.value = value;
            if (chargeText != null) chargeText.text = Mathf.RoundToInt(currentGenerator.charge) + "%";
        }
    }

    public void ShowUI(Generator gen)
    {
        currentGenerator = gen;
        if (canvas != null) canvas.enabled = true;
    }

    public void HideUI()
    {
        currentGenerator = null;
        if (canvas != null) canvas.enabled = false;
    }
}