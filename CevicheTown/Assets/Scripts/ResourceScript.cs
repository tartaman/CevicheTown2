using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ResourceScript : Building
{
    [SerializeField]
    ResourcesDatabase database;
    [SerializeField]
    public int id;
    int quantity;
    private void Start()
    {
        quantity = 10;
    }
    private void Update()
    {

    }
    public override void Place()
    {
        base.Place();
        if (buildingType == TypeBuilding.Resource)
        {
            GridBuildingSystem.instance.Enviroment.Add(this);
        }
    }
}
