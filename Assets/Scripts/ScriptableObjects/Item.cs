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
    
    public virtual uint GetCount()
    {
        return 1;
    }

    public virtual void SetCount(uint minValue, uint maxValue)
    {
        
    }
    
    public virtual void SetCount(uint concreteValue)
    {
        
    }

    //public GameObject gamePrefab;
}