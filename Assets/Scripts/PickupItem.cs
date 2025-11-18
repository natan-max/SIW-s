using UnityEngine;
using TMPro;

public class PickUpItem : MonoBehaviour, IHoldable
{
    [Header("Тип предмету")]
    public bool isFuel = true;
    public float maxFuel = 100f;
    [HideInInspector] public float currentFuel;

    [Header("Заправка")]
    public float fillRate = 20f;

    [Header("UI каністри")]
    public Canvas fuelCanvas;
    public TMP_Text fuelText;

    private Rigidbody rb;
    private Collider col;
    private bool isFilling = false;
    private Player player;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
        currentFuel = maxFuel;

        if (fuelCanvas != null)
            fuelCanvas.enabled = false;

        UpdateFuelUI();
    }

    void Update()
    {
        if (isFilling && currentFuel > 0f)
            FillGenerator();

        UpdateFuelUI();
    }

    public void Interact()
    {
        player = FindObjectOfType<Player>();
        if (player == null) return;
        if (player.currentHeldItem != null) return;



        if (rb != null) rb.isKinematic = true;
        if (col != null) col.enabled = false;

        player.currentHeldItem = this;

        if (fuelCanvas != null)
            fuelCanvas.enabled = true;

        isFilling = false;
    }

    public void StartFilling()
    {
        if (currentFuel <= 0f) return;
        Generator generator = FindObjectOfType<Generator>();
        if (generator == null || generator.charge >= 100f) return;
        isFilling = true;
    }

    public void StopFilling() => isFilling = false;

    void FillGenerator()
    {
        Generator generator = FindObjectOfType<Generator>();
        if (generator == null) return;

        if (generator.charge >= 100f)
        {
            generator.charge = 100f;
            StopFilling();
            return;
        }

        float fuelToUse = fillRate * Time.deltaTime;
        float neededFuel = 100f - generator.charge;
        fuelToUse = Mathf.Min(fuelToUse, neededFuel, currentFuel);

        currentFuel -= fuelToUse;
        generator.charge += fuelToUse;

        if (generator.charge >= 100f || currentFuel <= 0f)
        {
            generator.charge = Mathf.Min(generator.charge, 100f);
            currentFuel = Mathf.Max(currentFuel, 0f);
            StopFilling();
        }
    }

    void UpdateFuelUI()
    {
        if (fuelText != null)
            fuelText.text = Mathf.RoundToInt(currentFuel / maxFuel * 100f) + "%";
    }

    public void Drop()
    {
        StopFilling();

        transform.SetParent(null);

        if (rb != null) rb.isKinematic = false;
        if (col != null) col.enabled = true;

        rb.AddForce(transform.forward * 2f + Vector3.up * 1f, ForceMode.Impulse);

        if (fuelCanvas != null)
            fuelCanvas.enabled = false;

        if (player != null && player.currentHeldItem == this)
            player.currentHeldItem = null;
    }
}
