using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    #region Singleton

    public static InventoryUI Instance;

    private void Awake()
    {
        Instance = this;

        inventory = Inventory.Instance;
        inventory.onInventoryChangeCallback += UpdateInventoryUI;
        slots = itemsContainer.GetComponentsInChildren<InventorySlot>();
    }

    #endregion

    [SerializeField] private Text goldCountText;

    private Inventory inventory;
    public Transform itemsContainer;
    private InventorySlot[] slots;

    public InventorySlot currentSelectedSlot;

    #region UI_Update_Logic

    private void UpdateInventoryUI()
    {
        goldCountText.text = Inventory.Instance.GetGoldCount().ToString();
        SelectNextSlot(false);

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

        SelectNextSlot(true);
    }

    private void OnEnable()
    {
        UpdateInventoryUI();
        StopWeaponEquipment();
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

    #endregion

    #region EquipZone

    [Header("Control Area")] [SerializeField]
    private Button useButton;

    [SerializeField] private Button dropButton;
    [SerializeField] private Button destroyButton;

    [Header("Equip Area")] [SerializeField]
    private Animator equipAnimator;

    [SerializeField] private Text hintSelectEquipText;
    [SerializeField] private Image backgroundOnEquipImage;
    [SerializeField] private Button weaponOneButton;
    [SerializeField] private Button weaponTwoButton;

    private Weapon cachedWeaponToEquip;

    private void StartWeaponEquipment()
    {
        hintSelectEquipText.gameObject.SetActive(true);
        backgroundOnEquipImage.gameObject.SetActive(true);
        cachedWeaponToEquip = slots[GetCurrentSlotIndex()].slotItem as Weapon;
        inventory.equipMode = true;
        equipAnimator.Play("Glow");
    }

    public void StopWeaponEquipment()
    {
        hintSelectEquipText.gameObject.SetActive(false);
        backgroundOnEquipImage.gameObject.SetActive(false);
        inventory.equipMode = false;
        equipAnimator.Play("Default");
    }

    public void EquipSlotClick()
    {
        var clickedButtonGO = EventSystem.current.currentSelectedGameObject;

        if (inventory.equipMode)
        {
            clickedButtonGO.transform.GetChild(0).GetComponent<Image>().sprite = cachedWeaponToEquip.inventorySprite;
            clickedButtonGO.transform.GetChild(0).GetComponent<Image>().color = Colors.playerDefaultUI;
            Inventory.Instance.EquipWeapon(GetCurrentSlotIndex());
            StopWeaponEquipment();
        }
        else
        {
            // ToDo Fill for general usage
        }
    }

    #endregion

    #region InfoWindow

    [Header("Info Area")] [SerializeField] private Text infoItemDamage;
    [SerializeField] private Text infoItemDescription;
    [SerializeField] private Text infoItemName;
    [SerializeField] private Image infoItemSprite;
    [SerializeField] private Text infoItemTypes;

    private void ActivateItemInfo(bool value)
    {
        useButton.gameObject.SetActive(value);
        dropButton.gameObject.SetActive(value);
        destroyButton.gameObject.SetActive(value);

        if (value)
        {
            var weapon = slots[GetCurrentSlotIndex()].slotItem as Weapon;
            if (weapon != null)
            {
                FillInfoWindowWithWEapon(weapon.inventorySprite, weapon.name, weapon.minDamage,
                    weapon.maxDamage, weapon.DamageType, weapon.Speed, weapon.Range,
                    weapon.description);
            }
        }
        else
        {
            ClearInfoWindow();
        }
    }

    private void FillInfoWindowWithWEapon(Sprite sprite, string name, uint minDamage, uint maxDamage,
        Weapon.DamageTypeEnum damageType, Weapon.SpeedEnum speed, Weapon.RangeEnum range,
        string description)
    {
        infoItemSprite.sprite = sprite;
        infoItemName.text = name;
        infoItemDamage.text = "<color=#AA3232FF>" + minDamage + " - " + maxDamage + "</color> DMG (" + damageType + ")";
        infoItemTypes.text = speed + " speed, " + range + " range";
        infoItemDescription.text = description;

        useButton.GetComponentInChildren<Text>().text = "Equip";
    }

    private void ClearInfoWindow()
    {
        infoItemSprite.sprite = null;
        infoItemName.text = string.Empty;
        infoItemDamage.text = string.Empty;
        infoItemTypes.text = string.Empty;
        infoItemDescription.text = string.Empty;

        useButton.GetComponentInChildren<Text>().text = "Use";
    }

    #endregion

    #region ButtonClicks

    public void SelectItem(InventorySlot slot)
    {
        slot.Select();
    }

    public void UseItem()
    {
        if (currentSelectedSlot != null)
        {
            if (slots[GetCurrentSlotIndex()].slotItem is Weapon)
            {
                StartWeaponEquipment();
            }
            else if (slots[GetCurrentSlotIndex()].slotItem is Consumable)
            {
                // ToDo Potion Equip mode
            }
        }
    }

    public void DropItem()
    {
        if (currentSelectedSlot != null)
        {
            Inventory.Instance.AddToDropList(GetCurrentSlotIndex());
        }
    }

    public void DestroyItem()
    {
        if (currentSelectedSlot != null)
        {
            Inventory.Instance.DestroyItem(GetCurrentSlotIndex());
        }
    }

    public void InventoryCloseClick()
    {
        inventory.GenerateIfDrop();
        UIGamePlay.Instance.InventoryClose();
    }

    private int GetCurrentSlotIndex()
    {
        int slotIndex = 0;
        for (int i = 0; i < slots.Length; i++)
        {
            if (currentSelectedSlot == slots[i])
            {
                slotIndex = i;
            }
        }

        return slotIndex;
    }

    #endregion
}