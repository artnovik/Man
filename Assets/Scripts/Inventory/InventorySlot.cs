using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : GeneralSlot
{
    public void AddItem(Item newItem)
    {
        slotItem = newItem;

        slotIcon.sprite = slotItem.itemSprite;
        slotIcon.enabled = true;
    }
}