using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemStack : Item
{
    private int amount;

    public int GetAmount()
    {
        return amount;
    }

    public void Add(int value)
    {
        amount += value;
    }

    public void SetRandomAmount(int minValue, int maxValue)
    {
        amount = Random.Range(minValue, maxValue + 1);
    }

    public void SetAmount(int specifiсValue)
    {
        amount = specifiсValue;
    }
}