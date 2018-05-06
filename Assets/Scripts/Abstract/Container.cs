using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Container : Interactable
{
    protected uint containerCapacity = 10; // Value for all containers in game (should be good)

    public ContainerTypeEnum.Enum containerType;

    public List<Item> containerItems = new List<Item>();

    protected void OpenContainer()
    {
        UIGamePlay.Instance.ContainerOpen();
        ContainerUI.Instance.InitializeContainerUI(this);
    }

    public void MoveAllItemsToInventory()
    {
        foreach (Item item in containerItems.ToList())
        {
            MoveItemToInventory(item);
        }
    }

    public void MoveItemToInventory(Item item)
    {
        if (item is Gold)
        {
            Inventory.Instance.AddGold(item.GetCount());
        }
        else
        {
            if (Inventory.Instance.IsFull())
            {
                return;
            }

            Inventory.Instance.AddItem(item);
        }

        ContainerUI.Instance.SelectNextSlot(false); // If any but first slot was active 
        containerItems.Remove(item);
        ContainerUI.Instance.UpdateContainerSlots(this);
        ContainerUI.Instance.SelectNextSlot(true); // If first slot was active 
        IfContainerEmpty();
    }

    protected virtual void IfContainerEmpty()
    {
        UIGamePlay.Instance.ContainerClose();
    }
}