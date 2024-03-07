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
    public bool isPrimary;
    private void Start()
    {
        producesId = InventoryManager1.instance.resources.resourcedata.Find(x => x.source.id == id).ID;
        quantity = 10;
        isPrimary = false;
    }
    private void Update()
    {
        if (quantity == 0)
        {
            GridBuildingSystem.SetTilesBlock(area, tileTypes.Accepted, GridBuildingSystem.instance.maintilemap);
            GridBuildingSystem.instance.Enviroment.Remove(this);
            Destroy(gameObject);
        }
        if (isPrimary)
        {

        }
    }
    public override void Place()
    {
        Vector3Int positionInt = GridBuildingSystem.instance.gridLayout.LocalToCell(transform.position);
        BoundsInt areaTemp = area;
        areaTemp.position = positionInt;
        Placed = true;
        GridBuildingSystem.instance.TakeArea(areaTemp);
        transform.localPosition = GridBuildingSystem.instance.gridLayout.CellToLocalInterpolated(positionInt + new Vector3(.5f, .5f, 0f));
        if (TypeBuilding.Deco == buildingType || TypeBuilding.Generative == buildingType)
            GridBuildingSystem.instance.placedBuildings.Add(this);
        if (buildingType == TypeBuilding.Resource)
        {
            GridBuildingSystem.instance.Enviroment.Add(this);
        }
    }

    public TileBase[] GetTilesAround()
    {
        TileBase[] tilesAround;
        int count = 0;
        foreach (var tile in currRange.allPositionsWithin)
        {

        }
    }
}
