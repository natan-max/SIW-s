using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GeneratorUI : MonoBehaviour
{
    [Header("UI")]
    public GameObject uiPanel;
    public Scrollbar fuelBar;           // тільки генератор
    public TMP_Text fuelPercentText;    // відсотки генератора

    [Header("Колір заряду")]
    public Color lowColor = Color.red;
    public Color midLowColor = new Color(1f, 0.65f, 0f);
    public Color midColor = Color.yellow;
    public Color midHighColor = new Color(0.737f, 0.753f, 0f);
    public Color highColor = Color.green;

    [Header("Посилання на генератор")]
    public Generator generator;

    [Header("Плавність анімації")]
    public float lerpSpeed = 3f;

    [Header("Автоматичне приховання UI")]
    public Transform player;
    public float hideDistance = 5f;

    void Start()
    {
        if (uiPanel != null)
            uiPanel.SetActive(false);
    }

    void Update()
    {
        if (generator == null || fuelBar == null) return;

        float target = generator.charge / 100f;
        fuelBar.size = Mathf.Lerp(fuelBar.size, target, Time.deltaTime * lerpSpeed);

        // Змінюємо колір
        Image handleImage = fuelBar.handleRect.GetComponent<Image>();
        if (handleImage != null)
        {
            float normalized = fuelBar.size;
            if (normalized <= 0.1f) handleImage.color = lowColor;
            else if (normalized <= 0.25f) handleImage.color = midLowColor;
            else if (normalized <= 0.5f) handleImage.color = midColor;
            else if (normalized <= 0.75f) handleImage.color = midHighColor;
            else handleImage.color = highColor;
        }

        // Відображаємо відсотки генератора
        if (fuelPercentText != null)
            fuelPercentText.text = Mathf.RoundToInt(generator.charge) + "%";

        // Автоматичне ховання UI при відході
        if (uiPanel.activeSelf && player != null)
        {
            float distance = Vector3.Distance(player.position, generator.transform.position);
            if (distance > hideDistance)
                uiPanel.SetActive(false);
        }
    }

    public void ToggleUI()
    {
        if (uiPanel != null)
            uiPanel.SetActive(!uiPanel.activeSelf);
    }
}
