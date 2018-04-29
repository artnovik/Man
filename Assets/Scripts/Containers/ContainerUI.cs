using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContainerUI : MonoBehaviour
{
    #region Singleton

    public static ContainerUI Instance;

    private void Awake()
    {
        Instance = this;

        slots = gameObject.GetComponentsInChildren<ContainerSlot>();
    }

    #endregion

    [SerializeField] private Text nameText;
    [SerializeField] private Button closeButton;
    [SerializeField] private Button takeButton;
    [SerializeField] private Button takeAllButton;
    private ContainerSlot[] slots;

    public Item currentClickedItem;

    public void UpdateContainerUI(List<Item> itemsList)
    {
        for (var i = 0; i < slots.Length; i++)
        {
            if (i < 10)
            {
                slots[i].AddItem(itemsList[i]);
            }
        }
    }
}