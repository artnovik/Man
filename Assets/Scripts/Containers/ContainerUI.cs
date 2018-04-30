using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

public class ContainerUI : MonoBehaviour
{
    #region Singleton

    public static ContainerUI Instance;

    private void Awake()
    {
        Instance = this;

        slots = slotsContainer.GetComponentsInChildren<ContainerSlot>();
    }

    #endregion

    [SerializeField] private Text nameText;
    [SerializeField] private Button closeButton;
    [SerializeField] private Button takeButton;
    [SerializeField] private Button takeAllButton;

    [SerializeField] private RectTransform slotsContainer;
    public ContainerSlot[] slots;

    public Item currentClickedItem;

    [SerializeField] private Container currentContainer;

    public void AssignContainer(Container container)
    {
        currentContainer = container;
    }

    public void UpdateContainerUI(List<Item> containerItems)
    {
        for (var i = 0; i < containerItems.Count; i++)
        {
            slots[i].AddItem(containerItems[i]);
            if (containerItems[i] is Gold)
                slots[i].countText.text = containerItems[i].GetCount().ToString();
            // No more than 10
        }

        MakeAllSlotsInactive();
    }

    public void MakeSlotActive(ContainerSlot clickedSlot)
    {
        MakeAllSlotsInactive();
        clickedSlot.slotButton.GetComponent<Image>().color = Colors.playerActiveUI;
        clickedSlot.countText.color = Color.white;

        ActivateItemInfo(true);
    }

    public void MakeAllSlotsInactive()
    {
        foreach (var slot in slots)
        {
            slot.slotButton.GetComponent<Image>().color = Colors.playerDefaultUI;
            slot.countText.color = Color.black;
            ActivateItemInfo(false);
        }
    }

    private void ActivateItemInfo(bool value)
    {
        takeButton.gameObject.SetActive(value);

        if (value)
        {
            /*FillInfoWindow(item.inventorySprite, item.name, item.minDamage,
                item.maxDamage, item.DamageType, item.Speed, item.Range,
                item.description);*/
        }
        else
        {
        }
    }

    public void TakeItem()
    {
        if (currentClickedItem is Gold)
        {
            Inventory.Instance.AddGold(currentClickedItem.GetCount());
        }
        else
        {
            Inventory.Instance.items.Add(currentClickedItem);
        }

        currentContainer.RemoveItem(currentClickedItem);

        foreach (var slot in slots)
        {
            if (currentClickedItem == slot.item)
            {
                slot.ClearSlot();
            }
        }

        MakeAllSlotsInactive();

        if (currentContainer.containerItems.Count == 0)
        {
            currentContainer.Destroy();
            UIGamePlay.Instance.ContainerClose();
        }
    }

    public void TakeAllItems()
    {
        for (int i = 0; i < currentContainer.containerItems.Count; i++)
        {
            Inventory.Instance.Add(currentContainer.containerItems[i]);
        }

        currentContainer.containerItems.Clear();

        foreach (var slot in slots)
        {
            slot.ClearSlot();
        }

        currentContainer.Destroy();
        UIGamePlay.Instance.ContainerClose();
    }
}