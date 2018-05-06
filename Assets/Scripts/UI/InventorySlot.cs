using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public Image icon;
    public Button slotButton;
    public Text countText;
    public Item slotItem;

    public void AddItem(Item newItem)
    {
        slotItem = newItem;

        icon.sprite = slotItem.inventorySprite;
        icon.enabled = true;
    }

    public void ClearSlot()
    {
        slotItem = null;

        icon.sprite = null;
        icon.enabled = false;
    }

    // Modify for all selectable Items
    public void Select()
    {
        if (slotItem != null)
        {
            InventoryUI.Instance.currentSelectedSlot = this;
            InventoryUI.Instance.MakeSlotActive(this);
        }
        else
        {
            InventoryUI.Instance.MakeAllSlotsInactive();
        }
    }
}