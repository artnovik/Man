using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    private WeaponStats weaponStats;
    public Button removeButton;

    public Image icon;

    public void AddItem(WeaponStats newWeapon)
    {
        weaponStats = newWeapon;

        icon.sprite = weaponStats.inventorySprite;
        icon.enabled = true;
        removeButton.interactable = true;
    }


    public void ClearSlot()
    {
        weaponStats = null;

        icon.sprite = null;
        icon.enabled = false;
        removeButton.interactable = false;
    }

    public void OnRemoveButtonClick()
    {
        Instantiate(weaponStats.gamePrefab, PlayerControl.Instance.dropItemsPoint.position, PlayerControl.Instance.dropItemsPoint.rotation);

        Inventory.Instance.Remove(weaponStats);
    }

    public void Select()
    {
        if (weaponStats != null)
        {
            InventoryUI.Instance.FillInfoWindow(weaponStats.inventorySprite, weaponStats.name, weaponStats.minDamage, weaponStats.maxDamage, weaponStats.DamageType, weaponStats.Speed, weaponStats.Range, weaponStats.description);
        }
        else
        {
            InventoryUI.Instance.ClearInfoWindow();
        }
    }
}
