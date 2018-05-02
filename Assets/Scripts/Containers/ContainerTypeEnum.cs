using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerTypeEnum : MonoBehaviour
{
    #region Singleton

    public static ContainerTypeEnum Instance;

    private void Awake()
    {
        Instance = this;
    }

    #endregion

    public enum Enum
    {
        Common_Chest = 0,
        Uncommon_Chest = 1,
        Rare_Chest = 2,
        Mythical_Chest = 3,
        Legendary_Chest = 4,
        Zombie = 5,
        Chester = 6
    }
}