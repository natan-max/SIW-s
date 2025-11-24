using UnityEngine;

public class FuelCanister : InventoryItem
{
    public float maxFuel = 100f;
    public float fillRate = 25f;
    public float useRange = 5f;
    public AudioSource fillAudioSource;
    public AudioClip fillLoopClip;
    public AudioClip fillEndClip;
    private float currentFuel;
    private bool isUsing = false;
    private bool isLoopPlaying = false;
    public GeneratorUI generatorUI;

    protected override void Start()
    {
        base.Start();
        currentFuel = maxFuel;
        canBeUsedWithLMB = true;
    }

    protected override void Update()
    {
        base.Update();
        if (IsHeld())
        {
            transform.rotation = Quaternion.LookRotation(Camera.main.transform.forward);
        }

        if (!isUsing) return;

        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, useRange))
        {
            Generator gen = hit.collider.GetComponent<Generator>();
            if (gen != null)
            {
                float fuelToAdd = fillRate * Time.deltaTime;
                fuelToAdd = Mathf.Min(fuelToAdd, 100f - gen.charge, currentFuel);
                gen.charge += fuelToAdd;
                currentFuel -= fuelToAdd;

                if (generatorUI != null) generatorUI.ShowUI(gen);
                PlayLoopClip();
                return;
            }
        }

        if (generatorUI != null) generatorUI.HideUI();
    }

    private bool IsHeld()
    {
        Player player = FindObjectOfType<Player>();
        if (player == null) return false;
        return player.currentHeldItem == this;
    }

    private void PlayLoopClip()
    {
        if (fillAudioSource == null || fillLoopClip == null) return;
        if (!isLoopPlaying)
        {
            fillAudioSource.clip = fillLoopClip;
            fillAudioSource.loop = true;
            fillAudioSource.Play();
            isLoopPlaying = true;
        }
    }
}