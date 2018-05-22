using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Container : Interactable
{
    protected uint containerCapacity = 10; // Value for all containers in game (should be good)

    public ContainerTypeEnum.Enum containerType;
    public GameObject associatedEnemyWeapon;

    public List<Item> containerItems = new List<Item>();

    protected void OpenContainer()
    {
        GameplayUI.Instance.ContainerOpen();
        ContainerUI.Instance.InitializeContainerUI(this);
    }

    public void MoveAllItemsToInventory()
    {
        for (int i = containerItems.Count - 1; i > -1; i--)
        {
            MoveItemToInventory(i);
        }
    }

    public void MoveItemToInventory(int itemIndex)
    {
        if (containerItems[itemIndex] is Gold)
        {
            var gold = (Gold) containerItems[itemIndex];
            int goldAmount = gold.GetGoldAmount();

            Inventory.Instance.AddGold(goldAmount);
        }
        else
        {
            if (Inventory.Instance.IsFull())
            {
                return;
            }

            // Check if we are taking Weapon, which is in enemy's hand
            if (containerItems[itemIndex] == associatedEnemyWeapon.GetComponent<WeaponData>().weaponData)
            {
                Destroy(associatedEnemyWeapon);
            }
                
            Inventory.Instance.AddItem(containerItems[itemIndex]);
        }

        ContainerUI.Instance.SelectNextSlot(false); // If any but first slot was active 
        containerItems.RemoveAt(itemIndex);
        ContainerUI.Instance.UpdateContainerSlots(this);
        ContainerUI.Instance.SelectNextSlot(true); // If first slot was active 
        IfContainerEmpty();
    }

    protected virtual void IfContainerEmpty()
    {
        GameplayUI.Instance.ContainerClose();
    }
}