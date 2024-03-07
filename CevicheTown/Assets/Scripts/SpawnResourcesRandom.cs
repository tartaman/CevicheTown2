using UnityEngine;
using UnityEngine.Tilemaps;

public class SpawnResourcesRandom : MonoBehaviour
{
    [SerializeField] GameObject[] ResourceGO;
    ResourceScript[] resourceScripts;

    void Start()
    {
        // Inicializa el array de resourceScripts con la longitud correcta
        resourceScripts = new ResourceScript[ResourceGO.Length];

        // Obtén los scripts ResourceScript de los GameObjects ResourceGO
        for (int i = 0; i < ResourceGO.Length; i++)
        {
            resourceScripts[i] = ResourceGO[i].GetComponent<ResourceScript>();
        }

        PlaceInitialResources();
    }

    public  void PlaceInitialResources()
    {
        // Obtén el Tilemap
        Tilemap tilemap = GridBuildingSystem.instance.maintilemap;
        BoundsInt bounds = tilemap.cellBounds;

        // Itera sobre todas las posiciones del Tilemap
        for (int x = bounds.xMin; x < bounds.xMax; x++)
        {
            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                Vector3Int tilePosition = new Vector3Int(x, y, 0);

                // Verifica si hay un tile en la posición actual del Tilemap
                if (tilemap.HasTile(tilePosition))
                {
                    // Selecciona aleatoriamente un GameObject de la lista de recursos disponibles
                    GameObject randomResourceGO = GetRandomResourceGO();
                    Debug.Log($"mi random resource fue: {randomResourceGO}");
                    // Coloca el recurso aleatorio en la posición actual
                    if (randomResourceGO != null)
                    {
                        GameObject resourceGO = Instantiate(randomResourceGO, tilemap.GetCellCenterWorld(tilePosition), Quaternion.identity);
                        ResourceScript resourceScriptInstance = resourceGO.GetComponent<ResourceScript>();
                        resourceScriptInstance.Place();

                        
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
