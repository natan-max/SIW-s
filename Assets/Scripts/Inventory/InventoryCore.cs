using System;
using System.Collections.Generic; 
using UnityEngine;


public class InventoryCore : MonoBehaviour
{
    public event Action OnChanged;

    public KeyCode[] SlotKeys = {
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

    private void Update()
    {
        HandleSlotsSwitch();
        HandleItemDrop();
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

        item.DisablePhysics();
        Transform tr = item.transform;
        tr.SetParent(Hand);
        tr.localPosition = Vector3.zero;
        tr.localRotation = Quaternion.identity;

        SelectItem(item);
        OnChanged?.Invoke();
    }

    public void SelectSlot(int index)
    {
        SelectedSlotIndex = index;

        // вимикаємо всі предмети в руці
        for (int i = 0; i < _slots.Length; i++)
        {
            if (_slots[i] != null)
            {
                _slots[i].gameObject.SetActive(false);
            }
        }

        // вмикаємо тільки вибраний
        SelectItem(SelectedItem);
        OnChanged?.Invoke();
    }

    public void DropItem()
    {
        if (!IsActiveSlotOccupied())
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

        item.gameObject.SetActive(true);
        item.OnSelected();
    }

    private void DropItem(InventoryItem item)
    {
        if (item == null)
            return;

        item.transform.SetParent(null);

        item.EnablePhysics();
        item.Rb.AddForce(item.transform.forward * 5f, ForceMode.Impulse);

        item.OnDrop();
    }
}
