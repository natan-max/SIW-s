using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonAnimation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Vector3 originalScale;
    [SerializeField] private float scaleMultiplier = 1.1f; // збільшення при наведенні
    [SerializeField] private float animationSpeed = 10f;

    private bool isHovered = false;
    private Image buttonImage;
    [SerializeField] private Color normalColor = Color.white;
    [SerializeField] private Color hoverColor = Color.cyan;

    private void Start()
    {
        originalScale = transform.localScale;
        buttonImage = GetComponent<Image>();
        buttonImage.color = normalColor;
    }

    private void Update()
    {
        if (isHovered)
        {
            // Плавно збільшуємо та змінюємо колір
            transform.localScale = Vector3.Lerp(transform.localScale, originalScale * scaleMultiplier, Time.unscaledDeltaTime * animationSpeed);
            buttonImage.color = Color.Lerp(buttonImage.color, hoverColor, Time.unscaledDeltaTime * animationSpeed);
        }
        else
        {
            // Плавно повертаємо до початкового стану
            transform.localScale = Vector3.Lerp(transform.localScale, originalScale, Time.unscaledDeltaTime * animationSpeed);
            buttonImage.color = Color.Lerp(buttonImage.color, normalColor, Time.unscaledDeltaTime * animationSpeed);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovered = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovered = false;
    }
}