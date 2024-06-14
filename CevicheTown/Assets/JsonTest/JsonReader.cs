using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class JsonReader : MonoBehaviour
{
    [SerializeField] public string jsonFile;
    [SerializeField] public aUser user;

    private void Awake()
    {
        jsonFile = File.ReadAllText($"{Application.persistentDataPath}/GridData.json");
    }

    private void Start()
    {
        user = JsonUtility.FromJson<aUser>(jsonFile);
    }

    public void ParseJson()
    {
        user = JsonUtility.FromJson<aUser>(jsonFile);
    }

}
