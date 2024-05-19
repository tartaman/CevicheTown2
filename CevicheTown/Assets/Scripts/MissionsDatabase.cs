using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public struct Item
{
    int _quantity;

    public int id { get; set; }
    public int quantity {
        get => _quantity;
        set
        {
            if(value < 0)
            {
                throw new ArgumentOutOfRangeException("Quantity must be greater or equal than 0");
            }
            else
            {
                _quantity = value;
            }
        } 
    }

    public Item(int id, int quantity)
    {
        this.id = id;
        this._quantity = 0;
        this.quantity = quantity;
    }
}

[Serializable]
public class Mission
{
    int _reward;
    public int reward { 
        get { return _reward; } 
        set { 
            if(value < 0)
                throw new ArgumentOutOfRangeException("Reward must be greather than 0");
            else
                _reward = value;
        } 
    }
    public List<Item> items { get;  set; }

    public Mission()
    {
        reward = 0;
        items = new List<Item>();
    }

    public Mission(int reward, List<Item> items)
    {
        this.reward = reward;
        this.items = items;
    }

    // Devuelve si ya se agregó ese objeto y su index en el arreglo
    public (bool, int) FindIfAlreadyAdded(int index)
    {
        for(int i = 0; i < items.Count; i++)
        {
            if (items[i].id == index)
            {
                return (true, i);
            }
        }
        return (false, -1);
    }
}

[CreateAssetMenu]
public class MissionsDatabase : ScriptableObject
{
    public List<Mission> missions;
}
