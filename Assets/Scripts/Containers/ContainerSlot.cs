using UnityEngine;
using UnityEngine.UI;

public class ContainerSlot : GeneralSlot
{
    public void FillSlot(Item newItem)
    {
        slotItem = newItem;

        if (newItem.GetCount() > 1) // Not bad, but need to be done better.
        {
            countText.text = newItem.GetCount().ToString();
        }

        slotIcon.sprite = slotItem.inventorySprite;
        slotIcon.enabled = true;
    }

    public void ClearSlot()
    {
        slotItem = null;
        countText.text = null;

        slotIcon.sprite = null;
        slotIcon.enabled = false;
    }

    public void Select()
    {
        if (slotItem != null)
        {
            ContainerUI.Instance.currentSelectedSlot = this;
            ContainerUI.Instance.MakeSlotActive(this);
        }
        else
        {
            ContainerUI.Instance.currentSelectedSlot = null;
            ContainerUI.Instance.MakeAllSlotsInactive();
        }
    }
}