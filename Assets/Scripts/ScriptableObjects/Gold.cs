using UnityEngine;

[CreateAssetMenu(fileName = "Gold", menuName = "Gold")]
public class Gold : Item
{
    public uint count;

    public override uint GetCount()
    {
        return count;
    }

    public override void SetCount(uint minValue, uint maxValue)
    {
        count = (uint) Random.Range(minValue, maxValue);
    }

    public override void SetCount(uint concreteValue)
    {
        count = concreteValue;
    }
}