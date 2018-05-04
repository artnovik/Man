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
        // ToDo Handle this
        if (IsChest())
        {
            GetComponent<ChestAnimation>().OpenChestAnimation();
            return;
        }

        OpenContainer();
    }

    public bool IsChest()
    {
        return containerType == ContainerTypeEnum.Enum.Common_Chest ||
               containerType == ContainerTypeEnum.Enum.Uncommon_Chest ||
               containerType == ContainerTypeEnum.Enum.Rare_Chest ||
               containerType == ContainerTypeEnum.Enum.Mythical_Chest ||
               containerType == ContainerTypeEnum.Enum.Legendary_Chest;
    }

    public void OpenContainer()
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
        CheckIfContainerEmpty();
    }

    private void CheckIfContainerEmpty()
    {
        if (containerItems.Count < 1)
        {
            SwitchInteractableState();

            // ToDO Make this better
            if (containerType == ContainerTypeEnum.Enum.Zombie || containerType == ContainerTypeEnum.Enum.Chester)
            {
                Destroy(gameObject);
            }

            UIGamePlay.Instance.ContainerClose();

            if (IsChest())
            {
                GetComponent<ChestAnimation>().EmptyChestAnimation();
            }
        }
    }

    public void ChestDestroy()
    {
        Destroy(GetComponent<Container>());
        Destroy(GetComponent<ChestAnimation>());
        Destroy(GetComponent<Animator>());
    }
}