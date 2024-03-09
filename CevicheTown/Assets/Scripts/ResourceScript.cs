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
    [SerializeField]
    List<Vector3Int> posTilesAround;
    public int producesId;
    public int quantity;
    public bool isPrimary;
    private bool isCoroutineRunning = false;
    [SerializeField]
    List<Vector3Int> alreadyPlaced;
    private void Start()
    {
        posTilesAround = new List<Vector3Int>();
        producesId = InventoryManager1.instance.resources.resourcedata.Find(x => x.source.id == id).ID;
        quantity = 10;
        isPrimary = false;
        StartCoroutine(GenerateTreesAround());
    }
    private void Update()
    {
        
        if (quantity == 0)
        {
            GridBuildingSystem.SetTilesBlock(area, tileTypes.Accepted, GridBuildingSystem.instance.maintilemap);
            GridBuildingSystem.instance.Enviroment.Remove(this);
            Destroy(gameObject);
        }
        if (isPrimary)
        {
            posTilesAround = GetTilesAround();
            if (!isCoroutineRunning)
            {
                StartCoroutine(GenerateTreesAround());
                isCoroutineRunning = true;
            }
        }
    }
    public override void Place()
    {
        Vector3Int positionInt = GridBuildingSystem.instance.gridLayout.LocalToCell(transform.position);
        BoundsInt areaTemp = area;
        areaTemp.position = positionInt;
        Placed = true;
        GridBuildingSystem.instance.TakeArea(areaTemp);
        transform.localPosition = GridBuildingSystem.instance.gridLayout.CellToLocalInterpolated(positionInt + new Vector3(.5f, .5f, 0f));
        if (TypeBuilding.Deco == buildingType || TypeBuilding.Generative == buildingType)
            GridBuildingSystem.instance.placedBuildings.Add(this);
        if (buildingType == TypeBuilding.Resource)
        {
            GridBuildingSystem.instance.Enviroment.Add(this);
        }
        
    }

    public List<Vector3Int> GetTilesAround()
    {
        BoundsInt buildingArea = new BoundsInt(GridBuildingSystem.instance.gridLayout.WorldToCell(transform.position), area.size);
        currRange = buildingArea;
        currRange.xMin -= Range;
        currRange.yMin -= Range;
        currRange.xMax += Range;
        currRange.yMax += Range;
        foreach (var tile in currRange.allPositionsWithin)
        {
            if (!posTilesAround.Contains(tile))
            {
                posTilesAround.Add(tile); 
            }
        }
        return posTilesAround;
    }
    public IEnumerator GenerateTreesAround()
    {
        foreach (var tile in posTilesAround)
        {
            if (isPrimary)
            {
                Vector3Int randomPos;
                do
                {
                  randomPos = posTilesAround[Random.Range(0, posTilesAround.Count)];
                } while (alreadyPlaced.Contains(randomPos));

                if (AcceptedTile == GridBuildingSystem.instance.maintilemap.GetTile(randomPos))
                {
                    GameObject resourceGO = Instantiate(gameObject, GridBuildingSystem.instance.maintilemap.CellToLocal(randomPos), Quaternion.identity);
                    ResourceScript resourceScriptInstance = resourceGO.GetComponent<ResourceScript>();
                    resourceScriptInstance.isPrimary = false;
                    resourceScriptInstance.area.position = randomPos;
                    resourceScriptInstance.SetSortingOrder();
                    resourceScriptInstance.Place();
                    GridBuildingSystem.instance.Enviroment.Add(resourceScriptInstance);
                    Debug.Log($"Voy a generar un arbol en {randomPos}");
                    yield return new WaitForSeconds(1f);
                    alreadyPlaced.Add(randomPos);
                } else
                {
                    GenerateTreesAround();
                }
                
            }
            else
            {
                yield return null;
            }
        }
        
    }
}
