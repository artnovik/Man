using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public WeaponStats weaponStats;

    private new string name;
    private string description;

    private Sprite inventorySprite;       // To display in Inventory
    private GameObject gamePrefab;        // To display in Game

    private int minDamage;
    private int maxDamage;

    private int staminaConsume;

    private WeaponStats.SpeedEnum Speed;
    private WeaponStats.RangeEnum Range;
    private WeaponStats.DamageTypeEnum DamageType;

    private void Awake()
    {
        name = weaponStats.name;
        description = weaponStats.description;

        inventorySprite = weaponStats.inventorySprite;
        gamePrefab = weaponStats.gamePrefab;

        minDamage = weaponStats.minDamage;
        maxDamage = weaponStats.maxDamage;

        staminaConsume = weaponStats.staminaConsume;

        Speed = weaponStats.Speed;
        Range = weaponStats.Range;
        DamageType = weaponStats.DamageType;
    }

    public int GetDamage()
    {
        return Random.Range(minDamage, maxDamage);
    }

    public string GetName()
    {
        return name;
    }
}
