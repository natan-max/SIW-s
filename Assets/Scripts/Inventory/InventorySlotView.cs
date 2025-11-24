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

    public void Display(InventoryItem item, bool isSelected)
    {
        if (item != null)
        {
            ItemIcon.sprite = item.Icon;
            ItemIcon.enabled = item.Icon != null;
        }
        else
        {
            ItemIcon.enabled = false;
        }
        SlotBackground.color = isSelected ? ActiveColor : NormalColor;
    }
}