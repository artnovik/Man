using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public Image icon;
    public Button removeButton;
    private Weapon weaponStats;

    public void AddItem(Weapon newWeapon)
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
        GameObject droppedWeapon = Instantiate(weaponStats.gamePrefab, PlayerControl.Instance.dropItemsPoint.position,
            PlayerControl.Instance.dropItemsPoint.rotation);
        CollectiblesManager.Instance.SetParentAsCollectible(droppedWeapon);

        Inventory.Instance.Remove(weaponStats);
    }

    public void Select()
    {
        if (weaponStats != null)
        {
            InventoryUI.Instance.FillInfoWindow(weaponStats.inventorySprite, weaponStats.name, weaponStats.minDamage,
                weaponStats.maxDamage, weaponStats.DamageType, weaponStats.Speed, weaponStats.Range,
                weaponStats.description);
        }
        else
        {
            InventoryUI.Instance.ClearInfoWindow();
        }
    }
}