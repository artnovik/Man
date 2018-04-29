using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public Image icon;
    private Item item;

    public void AddItem(Item newItem)
    {
        item = newItem;

        icon.sprite = item.inventorySprite;
        icon.enabled = true;
    }


    public void ClearSlot()
    {
        item = null;

        icon.sprite = null;
        icon.enabled = false;
    }

    public void OnRemoveButtonClick()
    {
        Inventory.Instance.Remove(item);
    }

    public void Select()
    {
        if (item != null)
        {
            /*InventoryUI.Instance.FillInfoWindow(item.inventorySprite, item.name, item.minDamage,
                item.maxDamage, item.DamageType, item.Speed, item.Range,
                item.description);*/
        }
        else
        {
            InventoryUI.Instance.ClearInfoWindow();
        }
    }
}