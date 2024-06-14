using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using SimpleJSON;
public class LoadExistingGame : MonoBehaviour
{
    [SerializeField]
    DatabaseLoader databaseLoader;
    [System.Serializable]
    public class BuildingList
    {
        public aBuilding[] building;
    }
    [System.Serializable]
    public class ResourcesList
    {
        public aResource[] resource;
    }

    public BuildingList buildings = new BuildingList();
    public ResourcesList resources = new ResourcesList();
    public void checkJSON()
    {

        if (databaseLoader.JSONDATA != null) 
        {
            string jsonBuildings = "\"buildings" + databaseLoader.JSONDATA.Split("buildings")[1];
            jsonBuildings = jsonBuildings.Split("resources")[0];
            string jsonresources = "\"resources\"" + databaseLoader.JSONDATA.Split("resources")[1];
            Debug.Log(jsonresources);
            resources = JsonUtility.FromJson<ResourcesList>(jsonresources);
            Debug.Log(jsonBuildings);
            buildings = JsonUtility.FromJson<BuildingList>(jsonBuildings);
            //Building[] buildings = JsonUtility.FromJson<Building[]>(jsonBuildings);
            //ResourceScript[] Resources = JsonUtility.FromJson<ResourceScript[]>(jsonresources);
        }
    }
}
