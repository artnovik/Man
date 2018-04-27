using System.Collections.Generic;
using UnityEngine;

public class Container : MonoBehaviour
{
    private uint containerCapacity = 10; // Value for all containers in game (should be good)

    private List<Item> itemsContainer = new List<Item>();
}