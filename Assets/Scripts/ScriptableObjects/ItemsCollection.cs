using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsCollection : MonoBehaviour
{
    #region Singleton

    public static ItemsCollection Instance;

    private void Awake()
    {
        Instance = this;
    }

    #endregion

    public Gold gold;
    public Weapon knife;
    public Weapon saw;
    public Weapon scythe_big;
    public Consumable potion;

    public Weapon GetWeapon(Weapon weapon)
    {
        return weapon;
    }
}