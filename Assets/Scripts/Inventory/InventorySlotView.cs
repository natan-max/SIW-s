using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlotView : MonoBehaviour
{
    public Color ActiveColor;
    public Color NormalColor;
    
    public Image SlotBackground;
    public Image ItemIcon;
    public TextMeshProUGUI SlotId;

    public void Setup(string slotId)
    {
        SlotId.text = slotId;
    }

    public Sprite GetItemIcon(InventoryItem item)
    {
        if (item == null)
            return null;

        return item.Icon;
    }

    public void Display(InventoryItem item, bool isSelected)
    {
        Sprite itemIcon = GetItemIcon(item);
        ItemIcon.sprite = itemIcon;
        ItemIcon.enabled = itemIcon != null;
        
        if (isSelected)
        {
            SlotBackground.color = ActiveColor;
        }
        else
        {
            SlotBackground.color = NormalColor;
        }
    }
}
