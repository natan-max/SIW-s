using UnityEngine;

public class InventoryItem : MonoBehaviour, IInteractable
{
    public Sprite Icon;
    public Rigidbody Rb;
    public Collider Collider;
    public bool canBeUsedWithLMB = false;
    public AudioSource audioSource;
    protected Transform playerCam;

    protected virtual void Start()
    {
        Rb = Rb == null ? GetComponent<Rigidbody>() : Rb;
        Collider = Collider == null ? GetComponent<Collider>() : Collider;
        if (Camera.main != null) playerCam = Camera.main.transform;
    }

    public virtual void OnPickup() { }

    public virtual void OnSelected() { }

    public virtual void OnDeselected() { }

    public virtual void OnDrop() { }

    public void Interact()
    {
        InventoryCore inventory = FindObjectOfType<InventoryCore>();
        if (inventory == null) return;
        if (!inventory.IsActiveSlotOccupied())
        {
            inventory.PickupItem(this);
        }
    }

    public void EnablePhysics()
    {
        if (Rb != null) Rb.isKinematic = false;
        if (Collider != null) Collider.enabled = true;
    }

    public void DisablePhysics()
    {
        if (Rb != null) Rb.isKinematic = true;
        if (Collider != null) Collider.enabled = false;
    }

    protected virtual void Update()
    {
       
    }

    public virtual void StartUse() { }

    public virtual void StopUse() { }

    public virtual void UseOnce() { }
}