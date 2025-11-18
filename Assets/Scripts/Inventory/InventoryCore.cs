using System;
using System.Collections.Generic;
using UnityEngine;

public class InventoryCore : MonoBehaviour
{
    public event Action OnChanged;

    public KeyCode[] SlotKeys = new []
    {
        KeyCode.Alpha1,
        KeyCode.Alpha2,
        KeyCode.Alpha3,
        KeyCode.Alpha4,
    };
    public KeyCode DropKey = KeyCode.Q;
    public Transform Hand;

    public int SelectedSlotIndex { get; private set; }
    public InventoryItem SelectedItem => _slots[SelectedSlotIndex];
    public IReadOnlyList<InventoryItem> Slots => _slots;
        
    private InventoryItem[] _slots;

    private void Awake()
    {
        _slots = new InventoryItem[SlotKeys.Length];
    }

    public void HandleSlotsSwitch()
    {
        for (int i = 0; i < SlotKeys.Length; i++)
        {
            if (Input.GetKeyDown(SlotKeys[i]))
            {
                SelectSlot(i);
            }
        }
    }

    public void HandleItemDrop()
    {
        if (Input.GetKeyDown(DropKey))
        {
            DropItem();
        }
    }

    public bool IsActiveSlotOccupied()
    {
        return SelectedItem != null;
    }

    public void PickupItem(InventoryItem item)
    {
        _slots[SelectedSlotIndex] = item;
        item.OnPickup();
        item.DisablePhysics();
        Transform itemTransform = item.transform;
        itemTransform.SetParent(Hand);
        itemTransform.localRotation = Quaternion.identity;
        itemTransform.localPosition = Vector3.zero;
        SelectItem(item);
        OnChanged?.Invoke();
    }

    public void SelectSlot(int index)
    {
        DeselectItem(SelectedItem);
        SelectedSlotIndex = index;
        SelectItem(SelectedItem);
        OnChanged?.Invoke();   
    }

    public void DropItem()
    {
        if (IsActiveSlotOccupied() == false)
            return;

        InventoryItem item = SelectedItem;
        _slots[SelectedSlotIndex] = null;
        DropItem(item);
        OnChanged?.Invoke();
    }

    private void SelectItem(InventoryItem item)
    {
        if (item == null)
            return;

        item.OnSelected();
        item.gameObject.SetActive(true);
    }

    private void DeselectItem(InventoryItem item)
    {
        if (item == null)
            return;
        
        item.OnDeselected();
        item.gameObject.SetActive(false);
    }

    private void DropItem(InventoryItem item)
    {
        if (item == null)
            return;
        
        item.transform.parent = null;
        item.OnDrop();
        item.EnablePhysics();
        item.Rb.AddForce(item.transform.forward * 5, ForceMode.Impulse);
    }
}
