using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreesAround : MonoBehaviour
{
    [SerializeField]
    public Dictionary<ResourceScript, ResourceScript> ChildrenToPrimary = new Dictionary<ResourceScript, ResourceScript>();
    public static TreesAround instance;
    private void Awake()
    {
        instance = this;
    }
}
