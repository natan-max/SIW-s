using UnityEngine;

public class CrosshairInteraction : MonoBehaviour
{
    [Header("Налаштування прицілу")]
    public Texture2D crosshairTexture;
    public float crosshairSize = 32f;

    [Header("Кольори індикації")]
    public Color normalColor = Color.white;
    public Color itemColor = Color.green;
    public Color interactColor = Color.yellow;

    [Header("Raycast")]
    public float interactionDistance = 3f;

    private Rect crosshairRect;
    private Color currentColor;
    private IInteractable currentInteractable;

    void Start()
    {
        float xMin = (Screen.width - crosshairSize) / 2;
        float yMin = (Screen.height - crosshairSize) / 2;
        crosshairRect = new Rect(xMin, yMin, crosshairSize, crosshairSize);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void OnGUI()
    {
        if (crosshairTexture != null)
        {
            Color oldColor = GUI.color;
            GUI.color = currentColor;
            GUI.DrawTexture(crosshairRect, crosshairTexture);
            GUI.color = oldColor;
        }
    }

    void Update()
    {
        currentColor = normalColor;
        currentInteractable = null;

        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactionDistance))
        {
            // Якщо предмет для підбирання
            if (hit.collider.CompareTag("Item"))
            {
                currentColor = itemColor;

                if (Input.GetKeyDown(KeyCode.E))
                {
                    Inventory.Instance.AddItem(hit.collider.gameObject);
                }
            }
            // Якщо інший інтерактивний об'єкт
            else if (hit.collider.CompareTag("Door") ||
                     hit.collider.CompareTag("Laptop") ||
                     hit.collider.CompareTag("Window") ||
                     hit.collider.CompareTag("Ventilation"))
            {
                currentColor = interactColor;

                currentInteractable = hit.collider.GetComponent<IInteractable>();
                if (currentInteractable != null && Input.GetKeyDown(KeyCode.E))
                {
                    currentInteractable.Interact();
                }
            }
        }
    }
}
