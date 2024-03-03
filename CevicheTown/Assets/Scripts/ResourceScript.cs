using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ResourceScript : Building
{
    [SerializeField]
    ResourcesDatabase database;
    [SerializeField]
    public int id;
    int quantity;
    private void Start()
    {
        quantity = 10;
    }
    private void Update()
    {
        if (Placed)
            CheckIfInsideRange();
    }
    private void CheckIfInsideRange()
    {
        foreach (Building building in GridBuildingSystem.instance.placedBuildings)
        {
            // Verificar si el edificio tiene el recurso necesario
            if (building.neededResourceId == id)
            {
                // Iterar sobre todas las posiciones dentro del rango del edificio
                foreach (var position in building.currRange.allPositionsWithin)
                {
                    // Verificar si la posición actual coincide con la posición del recurso
                    if (position == area.position)
                    {
                        // El recurso está dentro del rango del edificio
                        building.HasNeededResource = true;
                        Debug.Log($"Building at {building.area.position} has the needed resource.");
                    }
                }
            }
        }
    }
}
