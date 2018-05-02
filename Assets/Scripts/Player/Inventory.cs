using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    #region Singleton

    public static Inventory Instance;

    private void Awake()
    {
        Instance = this;
    }

    #endregion

    public delegate void OnItemChanged();

    public int inventoryCapacity = 0;

    public OnItemChanged onItemChangedCallback;
    public List<Item> items = new List<Item>();

    private uint gold;

    public void AddGold(uint count)
    {
        gold += count;

        onItemChangedCallback?.Invoke();
    }

    public void RemoveGold(uint count)
    {
        gold -= count;

        onItemChangedCallback?.Invoke();
    }

    public uint GetGold()
    {
        return gold;
    }

    public void AddItem(Item item)
    {
        items.Add(item);

        onItemChangedCallback?.Invoke();
    }

    public void RemoveItem(Item item)
    {
        items.Remove(item);

        onItemChangedCallback?.Invoke();
    }

    public bool IsFull()
    {
        if (items.Count >= inventoryCapacity)
        {
            UIGamePlay.Instance.DisplayMessage("Inventory is full.", Colors.redMessage, 2f, false);
            return true;
        }
        else
        {
            return false;
        }
    }
}