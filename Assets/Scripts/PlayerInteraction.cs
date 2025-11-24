using UnityEngine;
using UnityEngine.UI;

public class PlayerInteraction : MonoBehaviour
{
    public float interactRange = 3f;
    public KeyCode interactKey = KeyCode.E;
    public Image crosshair;
    public Color normalColor = default;
    public Color highlightColor = default;

    private Camera cam;
    private IInteractable currentTarget;

    void Start()
    {
        cam = Camera.main;
        if (crosshair != null) crosshair.color = normalColor;
    }

    void Update()
    {
        DetectInteractable();
        if (currentTarget != null && Input.GetKeyDown(interactKey))
        {
            currentTarget.Interact();
        }
    }

    void DetectInteractable()
    {
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, interactRange))
        {
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();
            if (interactable != null)
            {
                currentTarget = interactable;
                if (crosshair != null) crosshair.color = highlightColor;
                return;
            }
        }
        currentTarget = null;
        if (crosshair != null) crosshair.color = normalColor;
    }
}
