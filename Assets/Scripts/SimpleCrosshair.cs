using UnityEngine;
using UnityEngine.UI;

public class SimpleCrosshair : MonoBehaviour
{
    [Header("UI")]
    public Image crosshairImage; // посилання на UI Image прицілу
    public RectTransform canvasRect; // канва, де знаходиться приціл

    [Header("Налаштування")]
    public float normalSize = 20f; // звичайний розмір прицілу
    public float aimSize = 10f;    // розмір прицілу при прицілюванні
    public KeyCode aimKey = KeyCode.Mouse1; // клавіша прицілювання

    private Camera mainCamera;

    void Start()
    {
        if (crosshairImage == null)
        {
            Debug.LogError("Crosshair Image не призначено!");
            enabled = false;
            return;
        }

        mainCamera = Camera.main;
    }

    void Update()
    {
        if (crosshairImage == null) return;

        // прицілювання
        if (Input.GetKey(aimKey))
        {
            crosshairImage.rectTransform.sizeDelta = new Vector2(aimSize, aimSize);
        }
        else
        {
            crosshairImage.rectTransform.sizeDelta = new Vector2(normalSize, normalSize);
        }

        // приціл завжди по центру екрану
        Vector2 screenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);
        crosshairImage.rectTransform.position = screenCenter;
    }
}