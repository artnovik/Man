using UnityEngine;

[CreateAssetMenu(fileName = "Gold", menuName = "Gold")]
public class Gold : ItemStack
{
    public int GetGoldAmount()
    {
        return amount;
    }

    public override void Add(int value)
    {
        amount += value;
    }

    public override void SetRandomAmount(int minValue, int maxValue)
    {
        amount = Random.Range(minValue, maxValue + 1);
    }

    public override void SetAmount(int specifiсValue)
    {
        amount = specifiсValue;
    }
}