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
        inventorySlots = itemsContainer.GetComponentsInChildren<InventorySlot>();
        equipWeaponSlots = gameObject.GetComponentsInChildren<EquipWeaponSlot>();
    }

    #endregion

    [SerializeField] private Text goldCountText;

    private Inventory inventory;
    public Transform itemsContainer;
    private InventorySlot[] inventorySlots;
    [HideInInspector] public EquipWeaponSlot[] equipWeaponSlots;

    public InventorySlot currentInventorySlot;
    public EquipWeaponSlot currentEquipWeaponSlot;

    #region UI_Update_Logic

    private void UpdateInventoryUI()
    {
        goldCountText.text = Inventory.Instance.GetGoldCount().ToString();
        SelectNextSlot(false);

        for (var i = 0; i < inventorySlots.Length; i++)
        {
            if (i < inventory.items.Count)
            {
                inventorySlots[i].AddItem(inventory.items[i]);
            }
            else
            {
                inventorySlots[i].ClearSlot();
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
        inventorySlots[0].Select();
    }

    public void SelectNextSlot(bool isFirstSlotActive)
    {
        if (isFirstSlotActive)
        {
            foreach (InventorySlot slot in inventorySlots)
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
            for (int i = 0; i < inventorySlots.Length; i++)
            {
                if (inventorySlots[i] == currentInventorySlot)
                {
                    inventorySlots[i].ClearSlot();
                    MakeAllSlotsInactive();

                    if (i > 0)
                    {
                        inventorySlots[i - 1].Select();
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

    public void MakeSlotActive(EquipWeaponSlot clickedWeaponSlot)
    {
        MakeAllSlotsInactive();
        clickedWeaponSlot.GetComponent<Image>().color = Colors.playerActiveUI;

        ActivateItemInfo(true, clickedWeaponSlot.associatedWeapon);
    }

    public void MakeAllSlotsInactive()
    {
        foreach (InventorySlot inventorySlot in inventorySlots)
        {
            inventorySlot.slotButton.GetComponent<Image>().color = Colors.playerDefaultUI;
            inventorySlot.countText.color = Color.black;
        }

        foreach (var equipSlot in equipWeaponSlots)
        {
            equipSlot.GetComponent<Image>().color = Colors.playerDefaultUI;
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

    private void StartWeaponEquipment()
    {
        hintSelectEquipText.gameObject.SetActive(true);
        backgroundOnEquipImage.gameObject.SetActive(true);
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

    public void EquipSlotOnClick(GameObject clickedButtonGO, int equipButtonIndex)
    {
        // Behaviour - If we are equipping weapon
        if (inventory.equipMode)
        {
            clickedButtonGO.transform.GetChild(0).GetComponent<Image>().sprite =
                inventorySlots[GetCurrentSlotIndex()].slotItem.inventorySprite;
            clickedButtonGO.transform.GetChild(0).GetComponent<Image>().color = Colors.playerDefaultUI;
            Inventory.Instance.EquipWeapon(GetCurrentSlotIndex(), equipButtonIndex);

            StopWeaponEquipment();
            currentInventorySlot = null;
            MakeSlotActive(equipWeaponSlots[equipButtonIndex]);
        }
        // Behaviour - If we are just want to select equipped weapon
        else
        {
            if (equipWeaponSlots[equipButtonIndex].associatedWeapon == null)
            {
                MakeAllSlotsInactive();
                return;
            }

            MakeSlotActive(equipWeaponSlots[equipButtonIndex]);
            currentEquipWeaponSlot = equipWeaponSlots[equipButtonIndex];
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
            var weapon = inventorySlots[GetCurrentSlotIndex()].slotItem as Weapon;
            if (weapon != null)
            {
                FillInfoWindowWithWeapon(weapon.inventorySprite, weapon.name, weapon.minDamage,
                    weapon.maxDamage, weapon.DamageType, weapon.Speed, weapon.Range,
                    weapon.description);
            }
        }
        else
        {
            ClearInfoWindow();
        }
    }

    private void ActivateItemInfo(bool value, Weapon weapon)
    {
        useButton.gameObject.SetActive(value);

        dropButton.gameObject.SetActive(value);
        destroyButton.gameObject.SetActive(value);

        if (value)
        {
            if (weapon != null)
            {
                FillInfoWindowWithWeapon(weapon.inventorySprite, weapon.name, weapon.minDamage,
                    weapon.maxDamage, weapon.DamageType, weapon.Speed, weapon.Range,
                    weapon.description);

                useButton.GetComponentInChildren<Text>().text = "UnEquip";
            }
        }
        else
        {
            ClearInfoWindow();
        }
    }

    private void FillInfoWindowWithWeapon(Sprite sprite, string name, uint minDamage, uint maxDamage,
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
        if (currentInventorySlot != null)
        {
            if (inventorySlots[GetCurrentSlotIndex()].slotItem is Weapon)
            {
                StartWeaponEquipment();
                return;
            }
            else if (inventorySlots[GetCurrentSlotIndex()].slotItem is Consumable)
            {
                // ToDo: Potion Equip mode.
                return;
            }
        }
        // If we clicking on EquipSlot
        else if (currentEquipWeaponSlot != null)
        {
            Inventory.Instance.UnEquipWeapon(currentEquipWeaponSlot.equipSlotIndex);
            currentEquipWeaponSlot.ClearSlot();
        }
    }

    public void DropItem()
    {
        if (currentInventorySlot != null)
        {
            Inventory.Instance.AddToDropList(GetCurrentSlotIndex());
        }
    }

    public void DestroyItem()
    {
        if (currentInventorySlot != null)
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
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            if (currentInventorySlot == inventorySlots[i])
            {
                slotIndex = i;
            }
        }

        return slotIndex;
    }

    #endregion
}