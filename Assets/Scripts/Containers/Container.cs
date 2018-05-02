using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Container : Interactable
{
    private uint containerCapacity = 10; // Value for all containers in game (should be good)

    public ContainerTypeEnum.Enum containerType;

    public List<Item> containerItems = new List<Item>();

    protected override void Interact()
    {
        if (!canInteract)
            return;

        base.Interact();

        // Custom Implementation
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

        ContainerUI.Instance.SelectPreviousSlot(); // IF we click not on 1st item
        containerItems.Remove(item);
        ContainerUI.Instance.UpdateContainerSlots(this);
        //ContainerUI.Instance.SelectFirstSlot(); // IF we click on 1st item
        CheckIfContainerEmpty();
    }

    private void CheckIfContainerEmpty()
    {
        if (containerItems.Count < 1)
        {
            canInteract = false;

            // ToDO Make this better
            if (containerType == ContainerTypeEnum.Enum.Zombie || containerType == ContainerTypeEnum.Enum.Chester)
            {
                Destroy(gameObject);
            }

            UIGamePlay.Instance.ContainerClose();
        }
    }
}