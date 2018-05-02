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

    [Header("Container_UI")] [SerializeField]
    private Text nameText;

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

    public void InitializeContainerUI(Container container)
    {
        nameText.text = container.containerType.ToString();
        currentContainer = container;
        MakeAllSlotsInactive();
        UpdateContainerSlots();
    }

    public void UpdateContainerSlots()
    {
        for (var i = 0; i < currentContainer.containerItems.Count; i++)
        {
            slots[i].ClearSlot();
            slots[i].FillSlot(currentContainer.containerItems[i]);

            // Make this better
            /*if (currentContainer.containerItems[i] is Gold || currentContainer.containerItems[i] is Consumable)
            {
                slots[i].countText.text = currentContainer.containerItems[i].GetCount().ToString();
            }*/
        }
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
        foreach (ContainerSlot slot in slots)
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

    public void ItemTaken()
    {
        foreach (var slot in slots)
        {
            if (currentClickedItem == slot.slotItem)
            {
                slot.ClearSlot();
            }
        }

        MakeAllSlotsInactive();
    }
    
    public void TakeItemClick()
    {
        currentContainer.MoveItemToInventory(currentClickedItem);

        /*if (Inventory.Instance.items.Count >= Inventory.Instance.inventoryCapacity)
        {
            UIGamePlay.Instance.DisplayMessage("Inventory is full.", Colors.redMessage, 2f, false);
            return;
        }

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

        MakeAllSlotsInactive();*/

        //CheckIfContainerEmpty();
    }

    public void TakeAllItemsClick()
    {
        currentContainer.MoveAllItemsToInventory();

        /*for (int i = 0; i < currentContainer.containerItems.Count; i++)
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
        }*/

        //CheckIfContainerEmpty();
    }
}