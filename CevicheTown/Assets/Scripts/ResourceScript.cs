using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ResourceScript : Building
{
    private void Update()
    {
        if (Placed)
            Debug.Log("Tecnicamente soy un edificio");
    }
}
