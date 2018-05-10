using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipWeaponSlot : EquipSlot
{
    public int equipSlotIndex;
    public Weapon associatedWeapon;

    public void EquipSlotClick()
    {
        InventoryUI.Instance.EquipSlotOnClick(gameObject, equipSlotIndex);

        InventoryUI.Instance.currentEquipWeaponSlot = this;
    }

    public void ClearSlot()
    {
        associatedWeapon = null;

        slotIcon.sprite = null;
    }
}