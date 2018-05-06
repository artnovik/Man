using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    #region Singleton

    public static InventoryUI Instance;

    private void Awake()
    {
        Instance = this;

        inventory = Inventory.Instance;
        inventory.onItemChangedCallback += UpdateInventoryUI;
        slots = itemsContainer.GetComponentsInChildren<InventorySlot>();
    }

    #endregion

    [SerializeField] private Text infoItemDamage;
    [SerializeField] private Text infoItemDescription;
    [SerializeField] private Text infoItemName;
    [SerializeField] private Image infoItemSprite;
    [SerializeField] private Text infoItemTypes;

    [SerializeField] private Text goldCount;

    [SerializeField] private Button useButton;
    [SerializeField] private Button dropButton;
    [SerializeField] private Button destroyButton;

    private Inventory inventory;
    public Transform itemsContainer;
    private InventorySlot[] slots;

    public InventorySlot currentSelectedSlot;

    private void UpdateInventoryUI()
    {
        goldCount.text = Inventory.Instance.GetGoldCount().ToString();

        for (var i = 0; i < slots.Length; i++)
        {
            if (i < inventory.items.Count)
            {
                slots[i].AddItem(inventory.items[i]);
            }
            else
            {
                slots[i].ClearSlot();
            }
        }

        MakeAllSlotsInactive();
        SelectFirstSlot();
    }

    public void SelectItem(InventorySlot slot)
    {
        slot.Select();
    }

    public void UseItem()
    {
        UIGamePlay.Instance.DisplayMessage("Coming soon...", Colors.greenMessage, 2f, false);
    }

    public void DropItem()
    {
        Inventory.Instance.AddToDropList(currentSelectedSlot.slotItem);
    }

    public void DestroyItem()
    {
        // TODO Make by index
        Inventory.Instance.DestroyItem(currentSelectedSlot.slotItem);
    }

    private void SelectFirstSlot()
    {
        slots[0].Select();
    }

    public void SelectNextSlot(bool isFirstSlotActive)
    {
        if (isFirstSlotActive)
        {
            foreach (InventorySlot slot in slots)
            {
                if (slot.slotButton.GetComponent<Image>().color == Colors.playerActiveUI)
                {
                    return;
                }
            }

            SelectFirstSlot();
        }
        else
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i] == currentSelectedSlot)
                {
                    slots[i].ClearSlot();
                    MakeAllSlotsInactive();

                    if (i > 0)
                    {
                        slots[i - 1].Select();
                    }
                }
            }
        }
    }

    public void MakeSlotActive(InventorySlot clickedSlot)
    {
        MakeAllSlotsInactive();
        clickedSlot.slotButton.GetComponent<Image>().color = Colors.playerActiveUI;
        clickedSlot.countText.color = Color.white;

        ActivateItemInfo(true);
    }

    public void MakeAllSlotsInactive()
    {
        foreach (InventorySlot slot in slots)
        {
            slot.slotButton.GetComponent<Image>().color = Colors.playerDefaultUI;
            slot.countText.color = Color.black;
        }

        ActivateItemInfo(false);
    }

    private void ActivateItemInfo(bool value)
    {
        useButton.gameObject.SetActive(value);
        dropButton.gameObject.SetActive(value);
        destroyButton.gameObject.SetActive(value);

        if (value)
        {
            var weapon = currentSelectedSlot.slotItem as Weapon;
            if (weapon != null)
            {
                FillInfoWindow(weapon.inventorySprite, weapon.name, weapon.minDamage,
                    weapon.maxDamage, weapon.DamageType, weapon.Speed, weapon.Range,
                    weapon.description);
            }
        }
        else
        {
            ClearInfoWindow();
        }
    }

    private void FillInfoWindow(Sprite sprite, string name, uint minDamage, uint maxDamage,
        Weapon.DamageTypeEnum damageType, Weapon.SpeedEnum speed, Weapon.RangeEnum range,
        string description)
    {
        infoItemSprite.sprite = sprite;
        infoItemName.text = name;
        infoItemDamage.text = "<color=#AA3232FF>" + minDamage + " - " + maxDamage + "</color> DMG (" + damageType + ")";
        infoItemTypes.text = speed + " speed, " + range + " range";
        infoItemDescription.text = description;
    }

    public void ClearInfoWindow()
    {
        infoItemSprite.sprite = null;
        infoItemName.text = string.Empty;
        infoItemDamage.text = string.Empty;
        infoItemTypes.text = string.Empty;
        infoItemDescription.text = string.Empty;
    }

    public void InventoryCloseClick()
    {
        inventory.GenerateIfDrop();
        UIGamePlay.Instance.InventoryClose();
    }
}