using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Mission
{
    public int reward { get; set; }
    public List<(int, int)> items;

    public Mission()
    {
        reward = 0;
        items = new List<(int, int)>();
    }

    public Mission(int reward, List<(int, int)> items)
    {
        this.reward = reward;
        this.items = items;
    }
}

[CreateAssetMenu]
public class MissionsDatabase : ScriptableObject
{
    public List<Mission> missions;
}
