using UnityEngine;

public class Item : ScriptableObject
{
    public new string name;
    public Sprite inventorySprite;
    public string description;
    public uint buyValue;
    public uint sellValue;
    
    public enum ItemTypeEnum
    {
        Weapon = 0,
        Consumable = 1,
        Gold = 2
    }

    public ItemTypeEnum ItemType;

    //public GameObject gamePrefab;
}