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
            case ContainerTypeEnum.Enum.CommonChest:
                return null;
                break;
            case ContainerTypeEnum.Enum.UncommonChest:
                return null;
                break;
            case ContainerTypeEnum.Enum.RareChest:
                return null;
                break;
            case ContainerTypeEnum.Enum.MythicalChest:
                return null;
                break;
            case ContainerTypeEnum.Enum.LegendaryChest:
                return null;
                break;
            case ContainerTypeEnum.Enum.Chester:
                return null;
                break;
            default:
                Debug.Log("Check parameters");
                return null;
        }
    }

    private List<Item> GenerateZombieItems()
    {
        var zombieItemsList = new List<Item>();
        zombieItemsList.Add(GenerateGold(ContainerTypeEnum.Enum.Zombie));
        //zombieItemsList.Add(GenerateSimpleWeapon());
        zombieItemsList.Add(GenerateSimpleWeapon(ItemsCollection.Instance.scythe_big));

        return zombieItemsList;
    }

    private Gold GenerateGold(ContainerTypeEnum.Enum containerType)
    {
        var gold = Instantiate(ItemsCollection.Instance.gold);

        switch (containerType)
        {
            // Enemies
            case ContainerTypeEnum.Enum.Zombie:
                gold.SetCount(3, 6);
                return gold;
            case ContainerTypeEnum.Enum.Chester:
                return gold;

            // Chests
            case ContainerTypeEnum.Enum.CommonChest:
                return gold;
            case ContainerTypeEnum.Enum.UncommonChest:
                return gold;
            case ContainerTypeEnum.Enum.RareChest:
                return gold;
            case ContainerTypeEnum.Enum.MythicalChest:
                return gold;
            case ContainerTypeEnum.Enum.LegendaryChest:
                return gold;
            default:
                Debug.Log("Check parameters");
                return null;
        }
    }

    private Item GenerateSimpleWeapon()
    {
        int minSeed = 1;
        int maxSeed = 3;
        int randSeed = Random.Range(minSeed, maxSeed);

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

    private Item GenerateSimpleWeapon(Weapon weapon)
    {
        return weapon = ItemsCollection.Instance.GetWeapon(weapon);
    }
}