using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Base Abstract Class for All Slots
public abstract class Slot : MonoBehaviour
{
    public Item slotItem;
    
    public Image slotIcon;
    public Button slotButton;

    public void FillSlot(Item newItem)
    {
        slotItem = newItem;
        
    }
    
    public virtual void Select()
    {
        if (slotItem != null)
        {
            //InventoryUI.Instance.currentInventorySlot = this;
            //InventoryUI.Instance.MakeSlotActive(this);
        }
        else
        {
            //InventoryUI.Instance.MakeAllSlotsInactive();
        }
    }
    
    public virtual void ClearSlot()
    {
        slotItem = null;

        slotIcon.sprite = null;
    }
}