using System.Collections.Generic;
using UnityEngine;

public class Container : Interactable
{
    private uint containerCapacity = 10; // Value for all containers in game (should be good)

    public List<Item> containerItems = new List<Item>();

    public override void Interact()
    {
        base.Interact();

        // Custom Implementation
        UIGamePlay.Instance.ContainerOpen();
        ContainerUI.Instance.AssignContainer(this);
        ContainerUI.Instance.UpdateContainerUI(containerItems);
    }

    public void RemoveItem(Item itemToRemove)
    {
        containerItems.Remove(itemToRemove);
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
}