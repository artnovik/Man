using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : GeneralSlot
{
    // Modify for all selectable Items
    public override void Select()
    {
        if (slotItem != null)
        {
            InventoryUI.Instance.currentInventorySlot = this;
            InventoryUI.Instance.MakeSlotActive(this);
        }
        else
        {
            InventoryUI.Instance.MakeAllSlotsInactive();
        }
    }
}