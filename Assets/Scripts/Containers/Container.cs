using System.Collections.Generic;
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
        foreach (Item item in containerItems)
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

        containerItems.Remove(item);
        ContainerUI.Instance.ItemTaken();             // Move this
        ContainerUI.Instance.UpdateContainerSlots(); // to here
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