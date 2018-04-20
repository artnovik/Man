using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New weapon stats", menuName = "Weapon Stats")]
public class WeaponStats : ScriptableObject
{
    public new string name;
    public string description;

    public Sprite inventorySprite;
    public GameObject gamePrefab;

    public int minDamage;
    public int maxDamage;

    public int staminaConsume;

    public int buyValue;
    public int sellValue;

    public SpeedEnum Speed;
    public enum SpeedEnum
    {
        Fast = 0,
        Normal = 1,
        Slow = 2,
    }

    public RangeEnum Range;
    public enum RangeEnum
    {
        Small = 0,
        Middle = 1,
        Large = 2,
    }

    public DamageTypeEnum DamageType;
    public enum DamageTypeEnum
    {
        Normal = 0,
        Arcane = 1,
        Water = 2,
        Fire = 3,
    }
}
