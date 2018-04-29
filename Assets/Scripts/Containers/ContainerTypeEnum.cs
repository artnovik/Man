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
        Chest = 0,
        Zombie = 1,
        Chester = 2
    }
}