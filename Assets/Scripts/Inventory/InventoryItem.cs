using System;
using UnityEngine;

public abstract class InventoryItem : MonoBehaviour, IInteractable
{
    public Sprite Icon;
    public Rigidbody Rb;
    public Collider Collider;

    public virtual void OnSelected() {}
    public virtual void OnDeselected() {}
    public virtual void OnPickup() {}
    public virtual void OnDrop() {}
    
    public void Interact()
    {
        InventoryCore inventory = FindObjectOfType<InventoryCore>();

        if (inventory.IsActiveSlotOccupied() == false)
        {
            inventory.PickupItem(this);
        }
    }

    public void EnablePhysics()
    {
        Rb.isKinematic = false;
        Collider.enabled = true;
    }

    public void DisablePhysics()
    {
        Rb.isKinematic = true;
        Collider.enabled = false;
    }
    
#if UNITY_EDITOR
    private void OnValidate()
    {
        if (Rb == null)
        {
            Rb = GetComponent<Rigidbody>();
            UnityEditor.EditorUtility.SetDirty(this);
        }

        if (Collider == null)
        {
            Collider = GetComponent<Collider>();
            UnityEditor.EditorUtility.SetDirty(this);
        }
    }
#endif
}
