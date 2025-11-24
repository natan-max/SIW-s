using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public InventoryCore Inventory;
    public InventorySlotView SlotPrefab;
    public Transform Container;
    private InventorySlotView[] _slots;

    private void Start()
    {
        _slots = new InventorySlotView[Inventory.SlotKeys.Length];
        for (int i = 0; i < _slots.Length; i++)
        {
            InventorySlotView view = Instantiate(SlotPrefab, Container);
            _slots[i] = view;
            string id = $"{i + 1}";
            view.Setup(id);
        }
        Inventory.OnChanged += DisplayItems;
        DisplayItems();
    }

    public void DisplayItems()
    {
        for (int i = 0; i < _slots.Length; i++)
        {
            InventorySlotView view = _slots[i];
            InventoryItem item = Inventory.Slots[i];
            bool isSelected = Inventory.SelectedSlotIndex == i;
            view.Display(item, isSelected);
        }
    }
}