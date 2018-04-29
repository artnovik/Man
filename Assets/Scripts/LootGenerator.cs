using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class LootGenerator : MonoBehaviour
{
    #region Singleton

    public static LootGenerator Instance;

    private void Awake()
    {
        Instance = this;
    }

    #endregion

    public Item GenerateGold(ContainerTypeEnum.Enum containerType)
    {
        var gold = ScriptableObject.CreateInstance<Gold>();
        gold = ItemsCollection.Instance.gold;

        switch (containerType)
        {
            case ContainerTypeEnum.Enum.Zombie:
                gold.count = (uint) Random.Range(3, 6);
                return gold;
            case ContainerTypeEnum.Enum.Chest:
                return null;
            case ContainerTypeEnum.Enum.Chester:
                return null;
            default:
                Debug.Log("Check parameters");
                return null;
        }
    }

    public Item GenerateSimpleWeapon()
    {
        int minSeed = 1;
        int maxSeed = 3;

        int randSeed = Random.Range(minSeed, maxSeed);

        var weapon = ScriptableObject.CreateInstance<Weapon>();
        switch (randSeed)
        {
            case 1:
                weapon = ItemsCollection.Instance.knife;
                return weapon;
            case 2:
                weapon = ItemsCollection.Instance.saw;
                return weapon;
            case 3:
                weapon = ItemsCollection.Instance.scythe_big;
                return weapon;
            default:
                return weapon;
        }
    }

    public Item GenerateSimpleWeapon(int seed)
    {
        var weapon = ScriptableObject.CreateInstance<Weapon>();
        switch (seed)
        {
            case 1:
                weapon = ItemsCollection.Instance.knife;
                return weapon;
            case 2:
                weapon = ItemsCollection.Instance.saw;
                return weapon;
            case 3:
                weapon = ItemsCollection.Instance.scythe_big;
                return weapon;
            default:
                return weapon;
        }
    }

    public List<Item> GenerateZombieItems()
    {
        var zombieItemsList = new List<Item>();
        zombieItemsList.Add(GenerateGold(ContainerTypeEnum.Enum.Zombie));
        zombieItemsList.Add(GenerateSimpleWeapon());

        return zombieItemsList;
    }
}