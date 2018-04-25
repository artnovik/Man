using UnityEngine;

public class Weapon : MonoBehaviour
{
    private WeaponStats.DamageTypeEnum DamageType;
    private string description;
    private GameObject gamePrefab; // To display in Game

    private Sprite inventorySprite; // To display in Inventory
    private int maxDamage;

    private int minDamage;

    private new string name;
    private WeaponStats.RangeEnum Range;

    private WeaponStats.SpeedEnum Speed;

    private int staminaConsume;
    public WeaponStats weaponStats;

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