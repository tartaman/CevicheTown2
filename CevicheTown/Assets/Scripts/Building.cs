using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public class Building : MonoBehaviour
{
    [SerializeField]
    ResourcesDatabase database;
    //Encapsule el placed 
    public bool Placed { get; set; }
    // area de el building
    public BoundsInt area;
    //orden en las layers
    int order = 1;
    //costo de el edificio
    public float cost;
    //tipo de el building (Ver el enum)
    public TypeBuilding buildingType;
    //Cuanto tiempo tarda en dar un tick
    [SerializeField]
    public float generateDelay;
    //Cantidad de recursos que genera
    [SerializeField]
    public int generateAmount;
    //Si está seleccionado (Usado ppara que se ponga transparente [Ver CheckClick])
    [SerializeField]
    public bool selected;
    //Encampsulado de selected
    public bool Selected { get { return selected; } set { selected = value; } }
    //Material de seleccionado
    [SerializeField]
    Material selectedMaterial;
    //Material para cuando se quite el seleccionado
    [SerializeField]
    private Material selectedMaterialOverride;
    //Tile sobre la que se puede poner
    [SerializeField]
    public TileBase AcceptedTile;
    //Sprite de el material que produce
    [SerializeField]
    Sprite ProducedMaterialSprite;
    //Particula para lo que va a generar
    ParticleSystem particle;
    //Rango de el edificio
    [SerializeField]
    int range;
    //Area del rando
    public BoundsInt currRange;
    //Encapsulamiento del rango
    public int Range { get { return range; } }
    //Cosas o recursos dentro del rango
    public List<Vector3Int> withinRange;
    //Identificador del recurso que genera, en este caso es el nombre en la base de datos
    [SerializeField]
    public string neededResourceId;
    //booleano para saber si el recurso que necesita está dentro del rango
    [SerializeField]
    public bool HasNeededResource;
    //Lista de los recursos dentro del rango
    [SerializeField]
    private List<ResourceScript> ResourcesInsideRange;
    private void Start()
    {
        ProducedMaterialSprite = database.resourcedata.Find(x => x.Name == neededResourceId).sprite;
        HasNeededResource = false;
        selected = false;
        if (buildingType == TypeBuilding.Generative)
        {
            particle = GetComponentInChildren<ParticleSystem>();
            particle.transform.position = transform.position + new Vector3(0,4f,0);
            particle.textureSheetAnimation.SetSprite(0, ProducedMaterialSprite);
            particle.Pause();
            StartCoroutine(GenerateCurrency());
        } else
        {
            particle = null;
        }
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (selected)
            {
                selected = false;
            }
        }
        // Check for mouse clicks
        if (Input.GetMouseButtonDown(0) && !GridBuildingSystem.instance.isPlacing && !UpgradeScript.instance.isUpgrading && !EventSystem.current.IsPointerOverGameObject()) // Left mouse button
        {
            CheckMouseClick();
        }
        if (selected)
        {
            GetComponentInChildren<SpriteRenderer>().material = selectedMaterial;
            GridBuildingSystem.SetTilesBlock(currRange, tileTypes.White, GridBuildingSystem.instance.temptilemap);
        } else if (Placed)
        {
            GetComponentInChildren<SpriteRenderer>().material = selectedMaterialOverride;
        }
        if (buildingType == TypeBuilding.Deco || buildingType == TypeBuilding.Resource)
        {
            generateDelay = 0;
            generateAmount = 0;
        }
        if (Placed)
        {
            CheckIfInsideRange();
            if (!GridBuildingSystem.instance.isPlacing && !UpgradeScript.instance.isUpgrading)
            {
                GridBuildingSystem.SetTilesBlock(currRange, tileTypes.Empty, GridBuildingSystem.instance.temptilemap);
            }
            for (int i = 0; i < ResourcesInsideRange.Count; i++)
            {
                if (ResourcesInsideRange[i] == null)
                {
                    ResourcesInsideRange.RemoveAt(i);
                }
            }
            if (ResourcesInsideRange.Count == 0)
            {
                HasNeededResource = false;
            }
        }
    }

    public virtual void CheckMouseClick()
    {
        // Raycast from the mouse position
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

        // Check if the click hits this building and if it's placed and not already in upgrading mode
        if (hit.collider != null && hit.collider.gameObject == gameObject && Placed && !EventSystem.current.IsPointerOverGameObject() && buildingType == TypeBuilding.Generative)
        {
            GameObject.FindGameObjectWithTag("CinemachineCamera").GetComponent<CinemachineVirtualCamera>().Follow = transform;
            // Set this building as selected
            selected = true;
            // Pass the selected building to the UpgradeScript
            UpgradeScript.instance.selectedBuilding = this;
            // Activate upgrading mode in UpgradeScript
            UpgradeScript.instance.isUpgrading = true;
            // Debug message
            Debug.Log($"Building in {area.x}, {area.y} clicked!");

            // Return early since we handled the click
            return;
        } else if (EventSystem.current.IsPointerOverGameObject())
        {
            selected = true;
        } else
        {
            selected = false;
        }
    }

    #region Placement Method
    public bool CanBeplaced()
    {
        Vector3Int positionInt = GridBuildingSystem.instance.gridLayout.LocalToCell(transform.position);
        BoundsInt areaTemp = area;
        areaTemp.position = positionInt;

        if (GridBuildingSystem.instance.CantTakeArea(areaTemp))
        {
            return true;
        }
        return false;
    }

    public virtual void Place()
    {
        Vector3Int positionInt = GridBuildingSystem.instance.gridLayout.LocalToCell(transform.position);
        BoundsInt areaTemp = area;
        areaTemp.position = positionInt;
        Placed = true;
        GridBuildingSystem.instance.TakeArea(areaTemp);
        ShopController.instance.currency -= cost;
        ShopController.instance.missionProgress.money -= (int)cost;
        if (TypeBuilding.Deco == buildingType || TypeBuilding.Generative == buildingType)
            GridBuildingSystem.instance.placedBuildings.Add(this);
    }
    public int SetSortingOrder()
    {
        order = GridBuildingSystem.instance.gridLayout.LocalToCell(transform.position).y * -1 - GridBuildingSystem.instance.gridLayout.LocalToCell(transform.position).x;

        Renderer renderer = GetComponentInChildren<Renderer>();
        if (renderer != null)
        {
            renderer.sortingOrder = order ;
        }
        else
        {
            SpriteRenderer spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                spriteRenderer.sortingOrder = order ;
            }
        }
        return order;
    }
    #endregion
    private void CheckIfInsideRange()
    {
                // Iterar sobre todas las posiciones dentro del rango del edificio
        foreach (var position in currRange.allPositionsWithin)
        {
            foreach (ResourceScript resource in GridBuildingSystem.instance.Enviroment)
            {
                foreach (var RePos in resource.posTilesAround)
                {
                    // Verificar si la posición actual coincide con la posición del recurso
                    if (RePos == position)
                    {
                        if (InventoryManager1.instance.resources.resourcedata.Find(x=> x.source.id == resource.producesId).Name == neededResourceId)
                        {
                            // El recurso está dentro del rango del edificio
                            HasNeededResource = true;
                            if (!ResourcesInsideRange.Contains(resource))
                            {
                                ResourcesInsideRange.Add(resource);
                            }
                            Debug.Log($"Building at {area.position} has the needed resource.");
                        
                        } else
                        {
                            HasNeededResource = false;
                            ResourcesInsideRange.Clear();
                        }
                    }
                }
            }
        }
    }
    private IEnumerator GenerateCurrency()
    {
        while (true)
        {
            if (buildingType == TypeBuilding.Generative && Placed && HasNeededResource)
            {
                // Store initial position
                Vector3 initialPosition = transform.position;

                // Calculate target position for jump
                Vector3 targetPosition = initialPosition + new Vector3(0, 0.2f, 0); // Adjust the y value as needed

                // Perform jump animation
                float jumpDuration = 0.2f; // Adjust the duration of the jump as needed
                float elapsedTime = 0f;

                while (elapsedTime < jumpDuration)
                {
                    transform.position = Vector3.Lerp(initialPosition, targetPosition, elapsedTime / jumpDuration);
                    elapsedTime += Time.deltaTime;
                    yield return null;
                }

                // Return to initial position
                transform.position = initialPosition;
                particle.Emit(generateAmount);
                // Wait for generate delay
                foreach (var resource in ResourcesInsideRange)
                {
                    if (resource.quantity > 0)
                    {
                        resource.quantity--;
                        // Generate Resource
                        InventoryManager1.instance.resources.resourcedata.Find(x => x.Name == neededResourceId).quantity += generateAmount;
                        break;
                    }
                }
                yield return new WaitForSeconds(generateDelay);
            }
            else
            {
                yield return null;
            }
        }
    }
}

public enum TypeBuilding
{
    Deco,
    Generative,
    Resource,
    Bank
}