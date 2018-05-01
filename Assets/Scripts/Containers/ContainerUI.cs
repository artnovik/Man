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

    [Header("InfoWindow")] [SerializeField]
    private Text itemNameInfo;

    [SerializeField] private Image itemIconInfo;

    [SerializeField] private Text itemDescriptionInfo;

    [SerializeField] private RectTransform slotsContainer;
    public ContainerSlot[] slots;

    public Item currentClickedItem;

    public Container currentContainer;

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
        }

        ActivateItemInfo(false);
    }

    private void ActivateItemInfo(bool value)
    {
        takeButton.gameObject.SetActive(value);

        if (value)
        {
            FillInfoWindow(currentClickedItem.inventorySprite, currentClickedItem.name, currentClickedItem.description);
        }
        else
        {
            ClearInfoWindow();
        }
    }

    private void FillInfoWindow(Sprite itemSprite, string itemName, string itemDescription)
    {
        itemIconInfo.sprite = itemSprite;
        itemNameInfo.text = itemName;
        itemDescriptionInfo.text = itemDescription;
    }

    private void ClearInfoWindow()
    {
        itemIconInfo.sprite = null;
        itemNameInfo.text = null;
        itemDescriptionInfo.text = null;
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

        CheckIfContainerEmpty();
    }

    public void TakeAllItems()
    {
        for (int i = 0; i < currentContainer.containerItems.Count; i++)
        {
            if (currentContainer.containerItems[i] is Gold)
            {
                Inventory.Instance.AddGold(currentContainer.containerItems[i].GetCount());
            }
            else
            {
                Inventory.Instance.Add(currentContainer.containerItems[i]);
            }
        }

        currentContainer.containerItems.Clear();

        foreach (var slot in slots)
        {
            slot.ClearSlot();
        }

        CheckIfContainerEmpty();
    }

    private void CheckIfContainerEmpty()
    {
        if (currentContainer.containerItems.Count < 1)
        {
            currentContainer.Destroy();
            UIGamePlay.Instance.ContainerClose();
        }
    }
}