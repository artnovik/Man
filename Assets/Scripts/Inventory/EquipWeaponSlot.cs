using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipWeaponSlot : EquipSlot
{
    public int equipWeaponSlotIndex;
    public Weapon associatedWeapon;

    public void ClearSlot()
    {
        associatedWeapon = null;

        slotIcon.sprite = null;
    }

    public override void Select()
    {
        InventoryUI.Instance.EquipSlotOnClick(gameObject, equipWeaponSlotIndex);

        // ToDo: Proper working UnEquip and SwapWeapons in Inventory, when equipped slot selected again
        if (slotItem != null)
        {
            InventoryUI.Instance.currentEquipWeaponSlot = this;
            InventoryUI.Instance.MakeSlotActive(this);
        }
    }
}