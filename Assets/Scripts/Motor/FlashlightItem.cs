using UnityEngine;

public class FlashlightItem : InventoryItem
{
    public Light flashlightLight;

    protected override void Start()
    {
        base.Start();
        canBeUsedWithLMB = true;
        if (flashlightLight != null) flashlightLight.enabled = false;
    }

    public override void StartUse()
    {
        if (flashlightLight != null) flashlightLight.enabled = !flashlightLight.enabled;
        if (audioSource != null) audioSource.Play();
    }

    public override void StopUse() { }
}