using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipSlot : MonoBehaviour
{
    public int equipSlotIndex;

    public void EquipSlotClick()
    {
        InventoryUI.Instance.EquipSlotOnClick(EventSystem.current.currentSelectedGameObject, equipSlotIndex);
    }
}