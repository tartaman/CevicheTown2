using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ObjectsDatabase : ScriptableObject
{
    public List<Objectdata> objectsdata;
}

[Serializable]
public class Objectdata
{
    [field: SerializeField]
    public string Name { get; private set; }
    [field: SerializeField]
    public int ID { get; private set; }
    [field: SerializeField]
    public GameObject prefab { get; private set; }

}
