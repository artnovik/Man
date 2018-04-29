using System.Collections.Generic;
using UnityEngine;

public class Container : Interactable
{
    private uint containerCapacity = 10; // Value for all containers in game (should be good)

    public List<Item> containerItems = new List<Item>();

    public void FillContainer(List<Item> itemsList)
    {
        if (itemsList != null)
        {
            containerItems.AddRange(itemsList);
        }
    }

    public override void Interact()
    {
        base.Interact();

        // Custom Implementation
        UIGamePlay.Instance.ContainerOpen();
        ContainerUI.Instance.UpdateContainerUI(containerItems);
    }
}