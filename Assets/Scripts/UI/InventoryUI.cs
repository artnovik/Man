using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public Transform itemsContainer;
    private InventorySlot[] slots;
    private Inventory inventory;

    [SerializeField]
    private Image infoItemSprite;
    [SerializeField]
    private Text infoItemName;
    [SerializeField]
    private Text infoItemDamage;
    [SerializeField]
    private Text infoItemTypes;
    [SerializeField]
    private Text infoItemDescription;

    #region Singleton

    public static InventoryUI Instance;

    private void Awake()
    {
        Instance = this;
    }

    #endregion
    
    public void StartCall()
    {
        inventory = Inventory.Instance;
        inventory.onItemChangedCallback += UpdateInventoryUI;
        slots = itemsContainer.GetComponentsInChildren<InventorySlot>();
    }

    public void UpdateInventoryUI()
    {
        // Update MoneyCount

        for (int i = 0; i < slots.Length; i++)
        {
            if (i < inventory.weapons.Count)
            {
                slots[i].AddItem(inventory.weapons[i]);
            }
            else
            {
                slots[i].ClearSlot();
                if (inventory.weapons.Count == 0)
                {
                    ClearInfoWindow();
                }
            }
        }
    }

    public void SelectItem(InventorySlot slot)
    {
        slot.Select();
    }

    public void FillInfoWindow(Sprite sprite, string name, int minDamage, int maxDamage, WeaponStats.DamageTypeEnum damageType, WeaponStats.SpeedEnum speed, WeaponStats.RangeEnum range, string description)
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
}
