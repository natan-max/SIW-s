using System;
using UnityEngine;

public class InventoryCore : MonoBehaviour
{
    public event Action OnChanged;
    public KeyCode[] SlotKeys = { KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4 };
    public KeyCode DropKey = KeyCode.Q;
    public Transform Hand;
    public int SelectedSlotIndex { get; private set; }
    public InventoryItem SelectedItem => _slots != null && SelectedSlotIndex >= 0 && SelectedSlotIndex < _slots.Length ? _slots[SelectedSlotIndex] : null;
    public InventoryItem[] Slots => _slots;
    private InventoryItem[] _slots;

    private void Awake()
    {
        _slots = new InventoryItem[SlotKeys.Length];
    }

    private void Update()
    {
        HandleSlotSwitch();
        HandleItemUseInput();
        HandleDropInput();
    }

    private void HandleSlotSwitch()
    {
        for (int i = 0; i < SlotKeys.Length; i++)
        {
            if (Input.GetKeyDown(SlotKeys[i]))
                SelectSlot(i);
        }
    }

    private void HandleItemUseInput()
    {
        if (SelectedItem != null)
        {
            if (Input.GetMouseButtonDown(0))
                SelectedItem.StartUse();
            if (Input.GetMouseButtonUp(0))
                SelectedItem.StopUse();
        }
    }

    private void HandleDropInput()
    {
        if (Input.GetKeyDown(DropKey))
            DropItem();
    }

    public void HandleSlotsSwitch()
    {
        HandleSlotSwitch();
    }

    public void HandleItemDrop()
    {
        HandleDropInput();
    }

    public bool IsActiveSlotOccupied()
    {
        return SelectedItem != null;
    }

    public void PickupItem(InventoryItem item)
    {
        _slots[SelectedSlotIndex] = item;
        item.DisablePhysics();
        item.transform.SetParent(Hand);
        item.transform.localPosition = Vector3.zero;
        item.transform.localRotation = Quaternion.identity;
        for (int i = 0; i < _slots.Length; i++)
        {
            if (_slots[i] != null)
                _slots[i].gameObject.SetActive(i == SelectedSlotIndex);
        }
        item.OnPickup();
        item.OnSelected();
        OnChanged?.Invoke();
    }

    public void SelectSlot(int index)
    {
        if (index < 0 || index >= _slots.Length) return;
        SelectedSlotIndex = index;
        for (int i = 0; i < _slots.Length; i++)
        {
            InventoryItem it = _slots[i];
            if (it != null)
            {
                bool isSelected = i == SelectedSlotIndex;
                it.gameObject.SetActive(isSelected);
                if (isSelected) it.OnSelected(); else it.OnDeselected();
            }
        }
        OnChanged?.Invoke();
    }

    public void DropItem()
    {
        if (!IsActiveSlotOccupied()) return;
        InventoryItem item = SelectedItem;
        _slots[SelectedSlotIndex] = null;
        if (item != null)
        {
            item.transform.SetParent(null);
            item.EnablePhysics();
            item.gameObject.SetActive(true);
            if (item.Rb != null) item.Rb.AddForce(item.transform.forward * 2f + Vector3.up * 1f, ForceMode.Impulse);
            item.OnDrop();
        }
        OnChanged?.Invoke();
    }
}
