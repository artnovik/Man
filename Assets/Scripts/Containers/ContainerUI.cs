using UnityEngine;
using UnityEngine.UI;

public class ContainerUI : MonoBehaviour
{
    [SerializeField] private Text nameText;
    [SerializeField] private Button closeButton;
    [SerializeField] private Button takeButton;
    [SerializeField] private Button takeAllButton;

    [SerializeField] private Item currentClickedItem;

    private void UpdateContainerUI()
    {
        /*for (var i = 0; i < slots.Length; i++)
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
            }*/
    }
}