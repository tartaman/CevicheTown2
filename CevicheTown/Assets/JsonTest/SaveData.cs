using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

public class SaveData : MonoBehaviour
{
    [SerializeField] public aUser _user = new aUser();
    
    private void OnApplicationQuit()
    {
        this.SaveIntoJson();
    }

    public void SaveIntoJson()
    {
        _user.grid.buildings.Clear();
        _user.grid.resources.Clear();

        GridBuildingSystem grid = GameObject.Find("Grid").GetComponent<GridBuildingSystem>();

        foreach (Building b in grid.placedBuildings)
        {
            aBuilding _building = new aBuilding();

            _building.name = b.name;
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
            _user.grid.buildings.Add(_building);

        }

        foreach (ResourceScript r in grid.Enviroment)
        {

            aResource _resource = new aResource();

            _resource.name = r.name;
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
            _user.grid.resources.Add(_resource);
        }

        _user.Money = GameObject.FindWithTag("MissionsManager").GetComponent<MissionsManager>().missionProgress.money;

        Debug.Log(Application.persistentDataPath + "/GridData.json");
        string gridData = JsonUtility.ToJson(_user);
        StartCoroutine(GameObject.Find("GameManager").GetComponent<DatabaseLoader>().saveGame(_user.Name, _user.grid.fileName, gridData));
        System.IO.File.WriteAllText(Application.persistentDataPath + "/GridData.json", gridData);
    }

}

[System.Serializable]
public class aUser
{
    public string Name;
    public int Money;
    public aGrid grid;
}

[System.Serializable]
public class aGrid
{
    public string fileName;
    public List<aBuilding> buildings;
    public List<aResource> resources;
}

[System.Serializable]
public class aBuilding
{
    public string name;

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
    public string name;

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
