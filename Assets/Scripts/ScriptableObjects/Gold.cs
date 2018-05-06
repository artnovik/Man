using UnityEngine;

[CreateAssetMenu(fileName = "Gold", menuName = "Gold")]
public class Gold : Item
{
    public int count;

    public override int GetCount()
    {
        return count;
    }

    public override void SetCount(int minValue, int maxValue)
    {
        count = Random.Range(minValue, maxValue+1);
    }

    public override void SetCount(int concreteValue)
    {
        count = concreteValue;
    }
}