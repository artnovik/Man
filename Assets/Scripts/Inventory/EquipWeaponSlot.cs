using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipWeaponSlot : EquipSlot
{
    public int equipWeaponSlotIndex;

    public override void FillSlot(Item newItem)
    {
        base.FillSlot(newItem);
    }

    public override void ClearSlot()
    {
        base.ClearSlot();

        slotIcon.enabled = true;
    }

    public override void Select()
    {
        base.Select();

        InventoryUI.Instance.EquipSlotOnClick(this);

        // ToDo: Proper working UnEquip and SwapWeapons in Inventory, when equipped slot selected again
        if (slotItem != null)
        {
            InventoryUI.Instance.currentEquipWeaponSlot = this;
            InventoryUI.Instance.MakeSlotActive(this);
        }
        else
        {
            InventoryUI.Instance.MakeAllSlotsInactive();
        }
    }
}