using UnityEngine;
using TMPro;

public class Flashlight : MonoBehaviour, IInteractable
{
    [Header("Налаштування ліхтарика")]
    public float maxCharge = 100f;
    public float currentCharge = 100f;
    public float drainRate = 5f;
    public Light flashlightLight;
    public Canvas uiCanvas;
    public TMP_Text chargeText;
    public LayerMask takeLayer;

    private Rigidbody rb;
    private Collider col;
    private Player player;
    private bool isOn = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
        UpdateChargeUI();
        if (uiCanvas != null) uiCanvas.enabled = false;
    }

    void Update()
    {
        if (player != null && player.currentHeldItem == this)
        {
            if (Input.GetMouseButtonDown(0))
                ToggleFlashlight();

            if (isOn && currentCharge > 0f)
            {
                currentCharge -= drainRate * Time.deltaTime;
                currentCharge = Mathf.Max(currentCharge, 0f);
                if (currentCharge <= 0f) TurnOff();
                UpdateChargeUI();
            }
        }
    }

    public void Interact()
    {
        // Перевірка Layer
        if (((1 << gameObject.layer) & takeLayer) == 0) return;

        player = FindObjectOfType<Player>();
        if (player == null || player.currentHeldItem != null) return;

        // Беремо ліхтарик у руку


        if (rb != null) rb.isKinematic = true;
        if (col != null) col.enabled = false;

        player.currentHeldItem = this;
        if (uiCanvas != null) uiCanvas.enabled = true;
    }

    public void Drop()
    {
        TurnOff();

        transform.SetParent(null);
        if (rb != null) rb.isKinematic = false;
        if (col != null) col.enabled = true;

        rb.AddForce(transform.forward * 2f + Vector3.up * 1f, ForceMode.Impulse);

        if (uiCanvas != null) uiCanvas.enabled = false;

        if (player != null && player.currentHeldItem == this)
            player.currentHeldItem = null;
    }

    public void AddBattery(float amount)
    {
        currentCharge += amount;
        if (currentCharge > maxCharge)
            currentCharge = maxCharge;

        UpdateChargeUI();
    }

    void ToggleFlashlight()
    {
        if (currentCharge <= 0f)
        {
            TurnOff();
            return;
        }

        isOn = !isOn;
        if (flashlightLight != null)
            flashlightLight.enabled = isOn;
    }

    void TurnOff()
    {
        isOn = false;
        if (flashlightLight != null)
            flashlightLight.enabled = false;
    }

    void UpdateChargeUI()
    {
        if (chargeText != null)
            chargeText.text = Mathf.RoundToInt(currentCharge) + "%";
    }
}
