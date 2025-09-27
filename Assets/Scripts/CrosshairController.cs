using UnityEngine;
using UnityEngine.UI;

public class CrosshairController : MonoBehaviour
{
    [Header("UI")]
    public Image crosshairImage;
    public Color defaultColor = Color.white;
    public Color interactColor = Color.yellow;
    public Color pickupColor = Color.green;

    [Header("Налаштування")]
    public float interactRange = 3f;
    public LayerMask interactableLayer;

    void Update()
    {
        UpdateCrosshair();
    }

    void UpdateCrosshair()
    {
        Vector3 screenCenter = new Vector3(Screen.width / 2f, Screen.height / 2f, 0f);
        Ray ray = Camera.main.ScreenPointToRay(screenCenter);

        if (Physics.Raycast(ray, out RaycastHit hit, interactRange, interactableLayer))
        {
            // якщо предмети laptop / door / window / vent
            if (hit.collider.CompareTag("Laptop") ||
                hit.collider.CompareTag("Door") ||
                hit.collider.CompareTag("Window") ||
                hit.collider.CompareTag("Vent"))
            {
                crosshairImage.color = interactColor;
            }
            // якщо предмет можна взяти (Item)
            else if (hit.collider.CompareTag("Item"))
            {
                crosshairImage.color = pickupColor;
            }
            else
            {
                crosshairImage.color = defaultColor;
            }
        }
        else
        {
            crosshairImage.color = defaultColor;
        }
    }
}