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

    #region Gold

    private int gold;

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

    #endregion

    #region ItemsInGeneral

    public void AddItem(Item item)
    {
        items.Add(item);

        onInventoryChangeCallback?.Invoke();
    }

    public void DestroyItem(int itemSlotIndex)
    {
        items.RemoveAt(itemSlotIndex);

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

    #endregion

    #region Equipment

    private List<Weapon> equippedWeapons = new List<Weapon>(2);
    public bool equipMode;

    public void EquipWeapon(int weaponSlotIndex)
    {
        // ToDo For each equip slot by index
        if (equippedWeapons.Count < 2)
        {
            equippedWeapons.Add(items[weaponSlotIndex] as Weapon);
            DestroyItem(weaponSlotIndex);
            onInventoryChangeCallback.Invoke();
        }
    }

    public void UnEquipWeapon()
    {
    }

    #endregion

    #region DropItems

    public List<Item> droppedItems = new List<Item>();

    public void AddToDropList(int itemSlotIndex)
    {
        if (droppedItems.Count == 10)
        {
            UIGamePlay.Instance.DisplayMessage("You can drop only 10 items at once", Colors.redMessage, 2f, false);
            return;
        }

        droppedItems.Add(items[itemSlotIndex]);
        items.RemoveAt(itemSlotIndex);
        InventoryUI.Instance.SelectNextSlot(true); // ToDo TEMP CONVINIENCE


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

    #endregion
}