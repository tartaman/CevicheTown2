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
    public int producesId;
    public int quantity;
    private void Start()
    {

        quantity = 10;
    }
    private void Update()
    {
        if (quantity == 0)
        {
            GridBuildingSystem.SetTilesBlock(area, tileTypes.Accepted, GridBuildingSystem.instance.maintilemap);
            GridBuildingSystem.instance.Enviroment.Remove(this);
            Destroy(gameObject);
        }
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
