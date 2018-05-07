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

    public int inventoryCapacity = 27;

    public OnItemChanged onItemChangedCallback;
    public List<Item> items = new List<Item>();

    private int gold;

    public void AddGold(int count)
    {
        gold += count;

        onItemChangedCallback?.Invoke();
    }

    public void RemoveGold(int count)
    {
        int goldValueAfterRemove = gold - count;

        if (goldValueAfterRemove < 0)
        {
            UIGamePlay.Instance.DisplayMessage("Not enough gold", Colors.redMessage, 2f, false);
        }
        else
        {
            gold = goldValueAfterRemove;
            onItemChangedCallback?.Invoke();
        }
    }

    public int GetGoldCount()
    {
        return gold;
    }

    public void AddItem(Item item)
    {
        items.Add(item);

        onItemChangedCallback?.Invoke();
    }

    public void Equip(Weapon weaponToEquip)
    {
        
    }

    // ToDo All this by Index

    public List<Item> droppedItems = new List<Item>();

    public void AddToDropList(Item item)
    {
        if (droppedItems.Count == 10)
        {
            UIGamePlay.Instance.DisplayMessage("You can drop only 10 items at once", Colors.redMessage, 2f, false);
            return;
        }

        droppedItems.Add(item);
        items.Remove(item);

        // ToDo TEMP CONVINIENCE
        InventoryUI.Instance.SelectNextSlot(true);
        onItemChangedCallback?.Invoke();
    }

    public void GenerateIfDrop()
    {
        if (droppedItems.Count > 0)
        {
            ContainerGenerator.Instance.GenerateAndFillContainer(ContainerGenerator.Instance.containerJunkPrefab,
                PlayerData.Instance.transform, ContainerTypeEnum.Enum.Junk, droppedItems);
            droppedItems.Clear();
        }
    }

    public void DestroyItem(Item item)
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