﻿using UnityEngine;
using UnityEngine.UI;

public class ContainerSlot : MonoBehaviour
{
    public Item slotItem;

    public Image icon;
    public Button slotButton;
    public Text countText;

    public void FillSlot(Item newItem)
    {
        slotItem = newItem;

        if (newItem.GetCount() > 1)
        {
            countText.text = newItem.GetCount().ToString();
        }

        icon.sprite = slotItem.inventorySprite;
        icon.enabled = true;
    }


    public void ClearSlot()
    {
        slotItem = null;
        countText.text = null;

        icon.sprite = null;
        icon.enabled = false;
    }

    public void Select()
    {
        if (slotItem != null)
        {
            ContainerUI.Instance.currentClickedItem = slotItem;
            ContainerUI.Instance.MakeSlotActive(this);
        }
        else
        {
            ContainerUI.Instance.currentClickedItem = null;
            ContainerUI.Instance.MakeAllSlotsInactive();
        }
    }
}