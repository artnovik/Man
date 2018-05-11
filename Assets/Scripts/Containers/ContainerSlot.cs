using UnityEngine;
using UnityEngine.UI;

public class ContainerSlot : GeneralSlot
{
    public void FillSlot(Item newItem)
    {
        slotItem = newItem;

        var stackItem = newItem as ItemStack;
        if (stackItem != null)
        {
            countText.text = stackItem.GetAmount().ToString();
        }

        slotIcon.sprite = slotItem.itemSprite;
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