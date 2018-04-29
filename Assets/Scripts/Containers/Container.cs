using System.Collections.Generic;
using UnityEngine;

public class Container : MonoBehaviour
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
}