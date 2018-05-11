using UnityEngine;

public abstract class Item : ScriptableObject
{
    public new string name;
    public Sprite inventorySprite;
    public string description;

    public virtual int GetCount()
    {
        return 1;
    }

    public virtual void SetCount(int minValue, int maxValue)
    {
        // This meant to be overridden.
    }

    public virtual void SetCount(int concreteValue)
    {
        // This meant to be overridden.
    }

    //public GameObject gamePrefab;
}