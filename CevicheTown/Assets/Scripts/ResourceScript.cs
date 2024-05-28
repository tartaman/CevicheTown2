using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ResourceScript : Building
{
    [SerializeField]
    public int id;
    [SerializeField]
    public List<Vector3Int> posTilesAround;
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
                StartCoroutine(GenerateTreesAround(0f));
                isCoroutineRunning = true;
            }
        }
        //foreach (var resource in GridBuildingSystem.instance.Enviroment)
        //{
        //    if (!resource.isPrimary)
        //    {
        //        resource.Range = 0;
        //        resource.isPrimary = true;
        //    }
            
        //}
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
            foreach(var resource in posTilesAround)
            {
                if (resource == null)
                {
                    posTilesAround.Remove(tile);
                }
            }
        }
        return posTilesAround;
    }
    //public IEnumerator GenerateTreesAround()
    //{
    //    while (true)
    //    {
    //        if (isPrimary)
    //        {
    //            Vector3Int randomPos = posTilesAround[Random.Range(0, posTilesAround.Count)];

    //            if (AcceptedTile == GridBuildingSystem.instance.maintilemap.GetTile(randomPos))
    //            {
    //                GameObject resourceGO = Instantiate(gameObject, GridBuildingSystem.instance.maintilemap.CellToLocal(randomPos), Quaternion.identity);
    //                ResourceScript resourceScriptInstance = resourceGO.GetComponent<ResourceScript>();
    //                resourceScriptInstance.isPrimary = false;
    //                resourceScriptInstance.area.position = randomPos;
    //                resourceScriptInstance.SetSortingOrder();
    //                resourceScriptInstance.Place();
    //                GridBuildingSystem.instance.Enviroment.Add(resourceScriptInstance);
    //                TreesAround.instance.ChildrenToPrimary.Add(resourceScriptInstance, this);
    //                Debug.Log($"Voy a generar un arbol en {randomPos}");
    //                yield return new WaitForSeconds(3f);
    //                alreadyPlaced.Add(randomPos);
    //            } 
    //        }
    //        else
    //        {
    //            yield return new WaitForSeconds(3f);
    //        }
    //    }
        
    //}
    public IEnumerator GenerateTreesAround(float delayInSeconds)
    {
        while (true)
        {
            yield return new WaitForSeconds(delayInSeconds); // Esperar el tiempo especificado

            if (isPrimary)
            {
                GenerateTrees();
            }
        }
    }

    void GenerateTrees()
    {
        Vector3Int randomPos = posTilesAround[Random.Range(0, posTilesAround.Count)];

        if (AcceptedTile == GridBuildingSystem.instance.maintilemap.GetTile(randomPos))
        {
            GameObject resourceGO = Instantiate(gameObject, GridBuildingSystem.instance.maintilemap.CellToLocal(randomPos), Quaternion.identity);
            ResourceScript resourceScriptInstance = resourceGO.GetComponent<ResourceScript>();
            resourceScriptInstance.isPrimary = false;
            resourceScriptInstance.area.position = randomPos;
            resourceScriptInstance.SetSortingOrder();
            resourceScriptInstance.Place();
            //GridBuildingSystem.instance.Enviroment.Add(resourceScriptInstance);
            //Debug.Log($"Voy a generar un arbol en {randomPos}");
            alreadyPlaced.Add(randomPos);
        }
    }
}
