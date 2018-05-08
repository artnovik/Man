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

    public delegate void OnInventoryChange();

    public int inventoryCapacity = 27;

    public OnInventoryChange onInventoryChangeCallback;
    public List<Item> items = new List<Item>();

    private int gold;

    public bool equipMode;

    private List<Weapon> equippedWeapons = new List<Weapon>(2);

    public void AddGold(int count)
    {
        gold += count;

        onInventoryChangeCallback?.Invoke();
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
            onInventoryChangeCallback?.Invoke();
        }
    }

    public int GetGoldCount()
    {
        return gold;
    }

    public void AddItem(Item item)
    {
        items.Add(item);

        onInventoryChangeCallback?.Invoke();
    }

    public void EquipWeapon(Weapon weaponToEquip)
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
        onInventoryChangeCallback?.Invoke();
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

        onInventoryChangeCallback?.Invoke();
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