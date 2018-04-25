using UnityEngine;

[CreateAssetMenu(fileName = "New weapon stats", menuName = "Weapon Stats")]
public class WeaponStats : ScriptableObject
{
    public enum DamageTypeEnum
    {
        Normal = 0,
        Arcane = 1,
        Water = 2,
        Fire = 3
    }

    public enum RangeEnum
    {
        Small = 0,
        Middle = 1,
        Large = 2
    }

    public enum SpeedEnum
    {
        Fast = 0,
        Normal = 1,
        Slow = 2
    }

    public int buyValue;

    public DamageTypeEnum DamageType;
    public string description;
    public GameObject gamePrefab;

    public Sprite inventorySprite;
    public int maxDamage;

    public int minDamage;
    public new string name;

    public RangeEnum Range;
    public int sellValue;

    public SpeedEnum Speed;

    public int staminaConsume;
}