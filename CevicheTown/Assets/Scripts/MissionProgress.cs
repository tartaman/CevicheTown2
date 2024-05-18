using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;



[Serializable]
public class MissionsInfo
{
    public int level;
    public int maxLevel;
    public int maxQuantityOfItems;
    public int maxQuantityOfMissions;
    public List<int> missionsToLevelUp;
    public List<int> minOfItemsPerLevel;
    public List<int> maxOfItemsPerLevel;
    

    public MissionsInfo() 
    {
        level = 1;
        maxLevel = 3;
        maxQuantityOfItems = 2;
        maxQuantityOfMissions = 5;
        missionsToLevelUp = new List<int> { 4, 12, 20};
        minOfItemsPerLevel = new List<int> { 100, 200, 300 };
        maxOfItemsPerLevel = new List<int> { 200, 300, 400 };

    }

    public MissionsInfo(int level, int maxLevel, int maxQuantityOfItems, List<int> missionsToLevelUp, List<int> minOfItemsPerLevel, List<int> maxOfItemsPerLevel, int maxQuantityOfMissions)
    {
        this.level = level;
        this.maxLevel = maxLevel;
        this.maxQuantityOfItems = maxQuantityOfItems;
        this.missionsToLevelUp = missionsToLevelUp;
        this.minOfItemsPerLevel = minOfItemsPerLevel;
        this.maxOfItemsPerLevel = maxOfItemsPerLevel;
        this.maxQuantityOfMissions = maxQuantityOfMissions;
    }

}


[CreateAssetMenu]
public class MissionProgress : ScriptableObject
{
    public MissionsInfo missionsInfo;

    public MissionProgress()
    {
        missionsInfo = new MissionsInfo();
    }
}