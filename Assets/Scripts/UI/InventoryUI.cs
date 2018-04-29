using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private Text infoItemDamage;
    [SerializeField] private Text infoItemDescription;
    [SerializeField] private Text infoItemName;
    [SerializeField] private Image infoItemSprite;
    [SerializeField] private Text infoItemTypes;

    private Inventory inventory;
    public Transform itemsContainer;
    private InventorySlot[] slots;

    private void UpdateInventoryUI()
    {
        // ToDO Update MoneyCount

        for (var i = 0; i < slots.Length; i++)
            if (i < inventory.items.Count)
            {
                slots[i].AddItem(inventory.items[i]);
            }
            else
            {
                slots[i].ClearSlot();
                if (inventory.items.Count == 0)
                {
                    ClearInfoWindow();
                }
            }
    }

    public void SelectItem(InventorySlot slot)
    {
        slot.Select();
    }

    public void FillInfoWindow(Sprite sprite, string name, uint minDamage, uint maxDamage,
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
}