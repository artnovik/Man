using UnityEngine;

[CreateAssetMenu(fileName = "Gold", menuName = "Gold")]
public class Gold : ItemStack
{
    public int count;

    public int GetCount()
    {
        return count;
    }

    public void SetCount(int minValue, int maxValue)
    {
        count = Random.Range(minValue, maxValue+1);
    }

    public void SetCount(int concreteValue)
    {
        count = concreteValue;
    }
}