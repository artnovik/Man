using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public Image icon;
    public Button removeButton;
    private Weapon weapon;

    public void AddItem(Weapon newWeapon)
    {
        weapon = newWeapon;

        icon.sprite = weapon.inventorySprite;
        icon.enabled = true;
        removeButton.interactable = true;
    }


    public void ClearSlot()
    {
        weapon = null;

        icon.sprite = null;
        icon.enabled = false;
        removeButton.interactable = false;
    }

    public void OnRemoveButtonClick()
    {
        Inventory.Instance.Remove(weapon);
    }

    public void Select()
    {
        if (weapon != null)
        {
            InventoryUI.Instance.FillInfoWindow(weapon.inventorySprite, weapon.name, weapon.minDamage,
                weapon.maxDamage, weapon.DamageType, weapon.Speed, weapon.Range,
                weapon.description);
        }
        else
        {
            InventoryUI.Instance.ClearInfoWindow();
        }
    }
}