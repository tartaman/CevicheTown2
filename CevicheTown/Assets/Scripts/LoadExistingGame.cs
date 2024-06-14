using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using System.IO;
using SimpleJSON;
using System.Threading;
public class LoadExistingGame : MonoBehaviour
{
    [SerializeField]
    DatabaseLoader databaseLoader;
    [SerializeField]
    public JsonReader jsonReader;
    [SerializeField]
    public GridBuildingSystem grid;
    public ResourcesDatabase Resourcedatabase;
    public ObjectsDatabase buildingDatabase;
    public void LoadGame()
    {
        Time.timeScale = 1.0f;
        databaseLoader.StartCoroutine(databaseLoader.loadGame(databaseLoader.username, databaseLoader.fileName));
        Debug.Log("Loading game: " + databaseLoader.fileName);
        Thread.Sleep(500);
        foreach(var build in jsonReader.user.grid.buildings)
        {
            GameObject a = buildingDatabase.objectsdata.Find(x => x.Name == build.name).prefab;
            ResourceScript script = Instantiate(a.GetComponent<ResourceScript>());
            script.currRange = build.currRange;
            script.transform.position = build.Position;
            script.transform.rotation = build.Rotation;
            script.Place();
        }
        foreach (var re in jsonReader.user.grid.resources)
        {
            ResourceScript a = Resourcedatabase.resourcedata.Find(x => x.Name == re.name).prefab;
            a.currRange = re.currRange;
            a.transform.position = re.Position;
            a.transform.rotation = re.Rotation;
            a.Place();
        }
        
    }
    public void ResetGrid()
    {
        foreach (var thing in grid.placedBuildings)
        {
            thing.Remove();
        }
        foreach (var thing in grid.Enviroment)
        {
            thing.Remove();
            Destroy(thing.gameObject);
        }
    }
}
