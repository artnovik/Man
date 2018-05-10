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

    public List<Item> GenerateItems(ContainerTypeEnum.Enum containerType)
    {
        switch (containerType)
        {
            case ContainerTypeEnum.Enum.Zombie:
                return GenerateZombieItems();
            case ContainerTypeEnum.Enum.Common_Chest:
                return GenerateCommonChestItems();
            case ContainerTypeEnum.Enum.Uncommon_Chest:
                return null;
            case ContainerTypeEnum.Enum.Rare_Chest:
                return null;
            case ContainerTypeEnum.Enum.Mythical_Chest:
                return null;
            case ContainerTypeEnum.Enum.Legendary_Chest:
                return null;
            case ContainerTypeEnum.Enum.Chester:
                return null;
            default:
                Debug.Log("Check parameters");
                return null;
        }
    }

    private List<Item> GenerateCommonChestItems()
    {
        var chestItemsList = new List<Item>();

        chestItemsList.Add(GenerateGold(ContainerTypeEnum.Enum.Common_Chest));
        chestItemsList.AddRange(RandomizeItems(3, 5));

        return chestItemsList;
    }

    private List<Item> GenerateZombieItems()
    {
        var zombieItemsList = new List<Item>();
        zombieItemsList.Add(GenerateGold(ContainerTypeEnum.Enum.Zombie));
        zombieItemsList.Add(GenerateRandomWeapon());

        return zombieItemsList;
    }

    private Gold GenerateGold(ContainerTypeEnum.Enum containerType)
    {
        Gold gold = Instantiate(ItemsCollection.Instance.gold);

        switch (containerType)
        {
            // Enemies
            case ContainerTypeEnum.Enum.Zombie:
                gold.SetCount(3, 6);
                return gold;
            case ContainerTypeEnum.Enum.Chester:
                return gold;

            // Chests
            case ContainerTypeEnum.Enum.Common_Chest:
                gold.SetCount(7, 18);
                return gold;
            case ContainerTypeEnum.Enum.Uncommon_Chest:
                return gold;
            case ContainerTypeEnum.Enum.Rare_Chest:
                return gold;
            case ContainerTypeEnum.Enum.Mythical_Chest:
                return gold;
            case ContainerTypeEnum.Enum.Legendary_Chest:
                return gold;
            default:
                Debug.Log("Check parameters");
                return null;
        }
    }

    private Item GenerateRandomWeapon()
    {
        int minSeed = 1;
        int maxSeed = 3;
        int randSeed = Random.Range(minSeed, maxSeed+1);

        switch (randSeed)
        {
            case 1:
                var weapon = ItemsCollection.Instance.knife;
                return weapon;
            case 2:
                weapon = ItemsCollection.Instance.saw;
                return weapon;
            case 3:
                weapon = ItemsCollection.Instance.scythe_big;
                return weapon;
            default:
                return null;
        }
    }

    private Item GenerateWeapon(Weapon weapon)
    {
        return weapon = ItemsCollection.Instance.GetWeapon(weapon);
    }

    // ToDo: Improve.
    private List<Item> RandomizeItems(uint minCount, uint maxCount)
    {
        var generatedItems = new List<Item>();
        var count = Random.Range(minCount, maxCount+1);

        for (int i = 0; i < count; i++)
        {
            generatedItems.Add(GenerateRandomWeapon());
        }

        return generatedItems;
    }
}