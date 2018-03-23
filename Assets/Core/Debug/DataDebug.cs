using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region ClassData

[System.Serializable]
public class DDebug
{
    public enum TColor
    {
        Error, Warning, Apply, Default, System
    }

    public TColor TypeColor;
    public Color32 Color;
}

#endregion

[System.Serializable]
public class DataDebug
{
    public List<DDebug> listDebug;

    private void Awake()
    {
        listDebug = new List<DDebug>();
    }
}
