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
        CommonChest = 0,
        UncommonChest = 1,
        RareChest = 2,
        MythicalChest = 3,
        LegendaryChest = 4,
        Zombie = 5,
        Chester = 6
    }
}