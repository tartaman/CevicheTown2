using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager1 : MonoBehaviour
{
    public static InventoryManager1 instance;
    [SerializeField]
    public ResourcesDatabase resources;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
