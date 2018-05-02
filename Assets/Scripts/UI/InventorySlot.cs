using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public Image icon;
    public Item slotItem;

    public void AddItem(Item newItem)
    {
        slotItem = newItem;

        icon.sprite = slotItem.inventorySprite;
        icon.enabled = true;
    }


    public void ClearSlot()
    {
        slotItem = null;

        icon.sprite = null;
        icon.enabled = false;
    }

    public void OnRemoveButtonClick()
    {
        Inventory.Instance.RemoveItem(slotItem);
    }

    public void Select()
    {
        if (slotItem != null)
        {
            /*InventoryUI.Instance.FillInfoWindow(item.inventorySprite, item.name, item.minDamage,
                item.maxDamage, item.DamageType, item.Speed, item.Range,
                item.description);*/

            InventoryUI.Instance.CurrentSelectedSlot = this;

            var weapon = slotItem as Weapon;
            if (weapon != null)
            {
                InventoryUI.Instance.FillInfoWindow(weapon.inventorySprite, weapon.name, weapon.minDamage,
                    weapon.maxDamage, weapon.DamageType, weapon.Speed, weapon.Range,
                    weapon.description);
            }
        }
        else
        {
            InventoryUI.Instance.ClearInfoWindow();
        }
    }
}