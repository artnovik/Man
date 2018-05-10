using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipSlot : MonoBehaviour
{
    public int equipSlotIndex;
    public Weapon associatedWeapon;

    public Image icon;

    public void EquipSlotClick()
    {
        InventoryUI.Instance.EquipSlotOnClick(gameObject, equipSlotIndex);

        InventoryUI.Instance.currentEquipSlot = this;
    }

    public void ClearSlot()
    {
        associatedWeapon = null;

        icon.sprite = null;
    }
}