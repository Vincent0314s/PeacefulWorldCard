using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class SinglePercentage {
    public string name;
    public int percentage;
}

[System.Serializable]
public class PercentageManager
{
    public List<SinglePercentage> percentageSection = new List<SinglePercentage>();
    [SerializeField]
    private int percentageSum;
    public void Initialization()
    {
        foreach (var item in percentageSection)
        {
            percentageSum += item.percentage;
        }
    }

    public string GetCertainPercentageFromList() {
        int randomNumber = Random.Range(0, percentageSum);
        foreach (var item in percentageSection)
        {
            if (randomNumber <= item.percentage)
            {
                return item.name;
            }
            else
            {
                randomNumber -= item.percentage;
            }
        }
        return null;
    }
}
