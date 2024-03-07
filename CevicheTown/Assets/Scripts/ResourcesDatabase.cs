using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ResourcesDatabase : ScriptableObject
{
    public List<Resourcedata> resourcedata;

    
}

[Serializable]
public class Resourcedata
{
    [field: SerializeField]
    public string Name { get; private set; }
    [field: SerializeField]
    public int ID { get; private set; }
    [field: SerializeField]
    public ResourceScript prefab { get; private set; }
    [field: SerializeField]
    GameObject sprite { get; set; }//Quiero que eso sea el sprite del recurso
    [field: SerializeField]
    public float ValuePerUnit { get; set; }
    [field: SerializeField]
    public int quantity { get; set; }
    [field: SerializeField]
    public ResourceScript source { get; private set; }
}
