using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager1 : MonoBehaviour
{
    List<Building> buildings;
    [SerializeField]
    ResourcesDatabase resources;
    // Start is called before the first frame update
    void Start()
    {
        buildings = GridBuildingSystem.instance.placedBuildings;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
