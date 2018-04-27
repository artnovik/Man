using UnityEngine;

public class Item : ScriptableObject
{
    public new string name;
    public Sprite inventorySprite;
    public string description;
    public uint buyValue;
    public uint sellValue;

    //public GameObject gamePrefab;
}