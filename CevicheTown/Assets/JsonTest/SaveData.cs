using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

public class SaveData : MonoBehaviour
{
    [SerializeField] aGrid _grid = new aGrid();

    public void SaveIntoJson()
    {
        GridBuildingSystem grid = GameObject.Find("Grid").GetComponent<GridBuildingSystem>();

        foreach(Building b in grid.placedBuildings)
        {
            while(b != null)
            {
                aBuilding _building = new aBuilding();

                _building.Position = b.gameObject.transform.localPosition;
                _building.Scale = b.gameObject.transform.localScale;
                _building.Rotation = b.gameObject.transform.localRotation;
                _building.cost = b.cost;
                _building.buildingType = b.buildingType;
                _building.generateDelay = b.generateDelay;
                _building.generateAmount = b.generateAmount;
                _building.Range = b.Range;
                _building.currRange = b.currRange;
                _building.needResourceId = b.neededResourceId;

                Debug.Log($"Saving {b.gameObject.name}");
                _grid.buildings.Add(_building);
            }    
        }

        foreach (ResourceScript r in grid.Enviroment)
        {
            while (r != null )
            {
                aResource _resource = new aResource();

                _resource.Position = r.transform.localPosition;
                _resource.Rotation = r.transform.localRotation;
                _resource.Scale = r.transform.localScale;

                _resource.cost = r.cost;
                _resource.resourceType = r.buildingType;
                _resource.generateDelay = r.generateDelay;
                _resource.generateAmount = r.generateAmount;
                _resource.Range = r.Range;
                _resource.currRange = r.currRange;
                _resource.producesId = r.producesId;
                _resource.quantity = r.quantity;

                Debug.Log($"Saving {r.gameObject.name}");
                _grid.resources.Add(_resource);
            } 
        }

        string gridData = JsonUtility.ToJson(_grid);
        System.IO.File.WriteAllText(Application.persistentDataPath + "/GridData.json", gridData);
    }
}

[System.Serializable]
public class aGrid
{
    public List<aBuilding> buildings;
    public List<aResource> resources;
}

[System.Serializable]
public class aBuilding
{
    public Vector2 Position;
    public Quaternion Rotation;
    public Vector2 Scale;

    public float cost;
    public TypeBuilding buildingType;
    public float generateDelay;
    public int generateAmount;

    public int Range;
    public BoundsInt currRange;
    public string needResourceId;
}

[System.Serializable]
public class aResource
{
    public int id;

    public Vector2 Position;
    public Quaternion Rotation;
    public Vector2 Scale;

    public float cost;
    public TypeBuilding resourceType;
    public float generateDelay;
    public int generateAmount;
    public int Range;
    public BoundsInt currRange;

    public int producesId;
    public int quantity;
}
