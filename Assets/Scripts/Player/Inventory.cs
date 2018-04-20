using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    // ToDo For "Items"

    #region Singleton

    public static Inventory Instance;

    private void Awake()
    {
        Instance = this;
    }

    #endregion

    public delegate void OnItemChanged();
    public OnItemChanged onItemChangedCallback;

    [HideInInspector]
    public int inventoryCapacity = 27;
    public List<WeaponStats> weapons = new List<WeaponStats>();

    public void Add(WeaponStats weapon)
    {
        weapons.Add(weapon);

        if (onItemChangedCallback != null)
            onItemChangedCallback.Invoke();
    }

    public void Remove(WeaponStats weapon)
    {
        weapons.Remove(weapon);

        if (onItemChangedCallback != null)
            onItemChangedCallback.Invoke();
    }
}
