using UnityEngine;
using UnityEngine.UI;

public class CrosshairController : MonoBehaviour
{
    [Header("Спрайт прицілу")]
    public Sprite crosshairSprite;

    [Header("Налаштування")]
    public Color defaultColor = Color.white;
    public Color interactColor = Color.yellow;
    public float interactRange = 3f;
    public KeyCode interactKey = KeyCode.E;

    [Header("Шари взаємодії")]
    public LayerMask openCloseLayer;
    public LayerMask takeLayer;
    public LayerMask onOffLayer;
    public LayerMask generatorLayer;

    private Camera mainCam;
    private Image crosshairImage;

    void Start()
    {
        mainCam = Camera.main;

        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            GameObject canvasGO = new GameObject("CrosshairCanvas");
            canvas = canvasGO.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasGO.AddComponent<CanvasScaler>();
            canvasGO.AddComponent<GraphicRaycaster>();
        }

        GameObject crosshairGO = new GameObject("Crosshair");
        crosshairGO.transform.SetParent(canvas.transform, false);

        crosshairImage = crosshairGO.AddComponent<Image>();
        crosshairImage.sprite = crosshairSprite;
        crosshairImage.color = defaultColor;

        RectTransform rt = crosshairGO.GetComponent<RectTransform>();
        rt.anchorMin = rt.anchorMax = new Vector2(0.5f, 0.5f);
        rt.anchoredPosition = Vector2.zero;
        rt.sizeDelta = crosshairSprite ? new Vector2(32, 32) : new Vector2(64, 64);
    }

    void Update()
    {
        if (mainCam == null || crosshairImage == null) return;

        Vector3 screenCenter = new Vector3(Screen.width / 2f, Screen.height / 2f, 0f);
        Ray ray = mainCam.ScreenPointToRay(screenCenter);
        RaycastHit hit;

        LayerMask[] layers = { openCloseLayer, takeLayer, onOffLayer, generatorLayer };
        bool interactableFound = false;

        foreach (LayerMask layer in layers)
        {
            if (Physics.Raycast(ray, out hit, interactRange, layer))
            {
                IInteractable interactable = hit.collider.GetComponentInParent<IInteractable>();
                if (interactable != null)
                {
                    crosshairImage.color = interactColor;
                    interactableFound = true;

                    if (Input.GetKeyDown(interactKey))
                        interactable.Interact();

                    break;
                }
            }
        }

        if (!interactableFound)
            crosshairImage.color = defaultColor;
    }
}
