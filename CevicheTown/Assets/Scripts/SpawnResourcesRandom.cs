using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SpawnResourcesRandom : MonoBehaviour
{
    [SerializeField] GameObject[] ResourceGO;
    ResourceScript[] resourceScripts;
    [SerializeField]
    int StartNumberOfResources;
    List<ResourceScript> primaryResources = new List<ResourceScript>();
    void Start()
    {
        // Inicializa el array de resourceScripts con la longitud correcta
        resourceScripts = new ResourceScript[ResourceGO.Length];

        // Obt�n los scripts ResourceScript de los GameObjects ResourceGO
        for (int i = 0; i < ResourceGO.Length; i++)
        {
            resourceScripts[i] = ResourceGO[i].GetComponent<ResourceScript>();
        }
        while (StartNumberOfResources > 0)
        {
            PlaceInitialResources();
        }
    }
    private void Update()
    {
        foreach (var resource in primaryResources) 
        { 
            resource.isPrimary = true;
        }
    }
    public  void PlaceInitialResources()
    {
        // Obt�n el Tilemap
        Tilemap tilemap = GridBuildingSystem.instance.maintilemap;
        BoundsInt bounds = tilemap.cellBounds;

        // Itera sobre todas las posiciones del Tilemap
        for (int x = bounds.xMin; x < bounds.xMax; x++)
        {
            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                Vector3Int tilePosition = new Vector3Int(Random.Range(bounds.xMin, bounds.xMax), Random.Range(bounds.yMin, bounds.yMax), 0);
                // Verifica si hay un tile en la posici�n actual del Tilemap
                if (tilemap.HasTile(tilePosition))
                {
                    // Selecciona aleatoriamente un GameObject de la lista de recursos disponibles
                    GameObject randomResourceGO = GetRandomResourceGO();
                    Debug.Log($"mi random resource fue: {randomResourceGO}");
                    // Coloca el recurso aleatorio en la posici�n actual
                    int rand = Random.Range(1, 11);
                    Debug.Log(rand);
                    if (randomResourceGO != null && rand == 1 && StartNumberOfResources > 0)//Dez percenche de probabilidade
                    {
                        GameObject resourceGO = Instantiate(randomResourceGO, tilemap.CellToLocal(tilePosition), Quaternion.identity);
                        ResourceScript resourceScriptInstance = resourceGO.GetComponent<ResourceScript>();
                        resourceScriptInstance.area.position = tilePosition;
                        resourceScriptInstance.isPrimary = true;
                        resourceScriptInstance.area.position = tilePosition;
                        bool hasAllAcceptedTiles = true;
                        foreach (var tile in tilemap.GetTilesBlock(resourceScriptInstance.area))
                        {
                            if (tile != resourceScriptInstance.AcceptedTile)
                            {
                                hasAllAcceptedTiles = false;
                            }
                        }

                        if (hasAllAcceptedTiles)
                        {
                            resourceScriptInstance.SetSortingOrder();
                            resourceScriptInstance.Place();
                            primaryResources.Add(resourceScriptInstance);
                            StartNumberOfResources--;
                        }
                        else 
                            Destroy(resourceGO);
                        
                    }
                }
            }
        }
    }
    GameObject GetRandomResourceGO()
    {
        if (ResourceGO.Length > 0)
        {
            int randomIndex = Random.Range(0, ResourceGO.Length);
            return ResourceGO[randomIndex];
        }
        else
        {
            return null;
        }
    }
}
