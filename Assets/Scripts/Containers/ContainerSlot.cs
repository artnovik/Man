using UnityEngine;
using UnityEngine.UI;

public class ContainerSlot : MonoBehaviour
{
    public Image icon;
    public Button slotButton;
    public Item item;
    public Text countText;

    public void AddItem(Item newItem)
    {
        item = newItem;

        icon.sprite = item.inventorySprite;
        icon.enabled = true;
    }


    public void ClearSlot()
    {
        item = null;
        countText.text = null;

        icon.sprite = null;
        icon.enabled = false;
    }

    public void Select()
    {
        if (item != null)
        {
            ContainerUI.Instance.currentClickedItem = item;
            ContainerUI.Instance.MakeSlotActive(this);
        }
        else
        {
            ContainerUI.Instance.currentClickedItem = null;
            ContainerUI.Instance.MakeAllSlotsInactive();
        }
    }
}