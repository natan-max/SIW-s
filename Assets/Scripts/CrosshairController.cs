using UnityEngine;
using UnityEngine.UI;

public class CrosshairController : MonoBehaviour
{
    [Header("UI")]
    public Image crosshairImage;
    public Color defaultColor = Color.white;
    public Color doorVentWindowColor = Color.yellow; // для Door / Vent / Window
    public Color laptopColor = Color.green;          // для Laptop
    public Color itemColor = Color.red;              // для Item

    [Header("Налаштування")]
    public float interactRange = 3f;
    public LayerMask interactableLayer; // для перевірки при натисканні E (залишити 0, щоб дозволити на будь-якому шарі)

    // внутрішні
    private RaycastHit lastHit;
    private bool hasHit = false;

    void Update()
    {
        UpdateCrosshair();
        HandleInteractInput();
    }

    void UpdateCrosshair()
    {
        Vector3 screenCenter = new Vector3(Screen.width / 2f, Screen.height / 2f, 0f);
        Ray ray = Camera.main.ScreenPointToRay(screenCenter);

        hasHit = Physics.Raycast(ray, out lastHit, interactRange);

        if (hasHit)
        {
            string tag = lastHit.collider.tag;

            if (tag == "Door" || tag == "Vent" || tag == "Window")
                crosshairImage.color = doorVentWindowColor;
            else if (tag == "Laptop")
                crosshairImage.color = laptopColor;
            else if (tag == "Item")
                crosshairImage.color = itemColor;
            else
                crosshairImage.color = defaultColor;
        }
        else
        {
            crosshairImage.color = defaultColor;
        }
    }

    void HandleInteractInput()
    {
        if (!hasHit) return;
        if (!Input.GetKeyDown(KeyCode.E)) return;

        GameObject hitObj = lastHit.collider.gameObject;

        // Якщо LayerMask не задано (значення 0) — дозволяємо взаємодіяти з будь-яким шаром.
        bool layerOk = (interactableLayer.value == 0) || ((interactableLayer.value & (1 << hitObj.layer)) != 0);
        if (!layerOk) return;

        // шукаємо компонент InteractableToggle (у самому колайдері або в батьках)
        InteractableToggle interactable = lastHit.collider.GetComponentInParent<InteractableToggle>();
        if (interactable != null)
        {
            interactable.Interact();
        }
        else
        {
            Debug.Log($"Об'єкт {hitObj.name} не має InteractableToggle.");
        }
    }
}
