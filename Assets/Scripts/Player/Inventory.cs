using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public delegate void OnItemChanged();

    [HideInInspector] public int inventoryCapacity = 27;

    public OnItemChanged onItemChangedCallback;
    public List<Item> items = new List<Item>();

    public void Add(Item item)
    {
        items.Add(item);

        if (onItemChangedCallback != null)
        {
            onItemChangedCallback.Invoke();
        }
    }

    public void Remove(Item item)
    {
        items.Remove(item);

        if (onItemChangedCallback != null)
        {
            onItemChangedCallback.Invoke();
        }
    }

    #region Singleton

    public static Inventory Instance;

    private void Awake()
    {
        Instance = this;
    }

    #endregion
}