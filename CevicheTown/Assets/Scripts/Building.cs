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
    public bool Placed { get; private set; }
    public BoundsInt area;
    int order = 1;
    public int ID;
    public float cost;
    public TypeBuilding buildingType;
    [SerializeField]
    public float generateDelay;
    [SerializeField]
    public int generateAmount;
    [SerializeField]
    bool selected;
    public bool Selected { get { return selected; } set { selected = value; } }
    [SerializeField]
    Material selectedMaterial;
    [SerializeField]
    private Material selectedMaterialOverride;
    [SerializeField]
    public TileBase AcceptedTile;
    [SerializeField]
    Sprite ProducedMaterialSprite;
    ParticleSystem particle;
    [SerializeField]
    int range;
    public BoundsInt currRange;
    public int Range { get { return range; } }
    public List<Vector3Int> withinRange;
    public Vector3Int positionInGrid;
    [SerializeField]
    public int neededResourceId;
    [SerializeField]
    public bool HasNeededResource;
    [SerializeField]
    private List<ResourceScript> ResourcesInsideRange;
    private void Start()
    {
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
        }
    }

    private void CheckMouseClick()
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
                // Verificar si la posición actual coincide con la posición del recurso
                if (resource.area.position == position)
                {
                    if (resource.id == neededResourceId)
                    {
                        // El recurso está dentro del rango del edificio
                        HasNeededResource = true;
                        if (!ResourcesInsideRange.Contains(resource))
                        {
                            ResourcesInsideRange.Add(resource);
                        }
                        Debug.Log($"Building at {area.position} has the needed resource.");
                        return;
                    }
                }
                HasNeededResource = false;
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
                yield return new WaitForSeconds(generateDelay);

                // Generate currency
                ShopController.instance.currency += generateAmount;
                foreach(var resource in ResourcesInsideRange)
                {
                    if (resource.quantity > 0)
                    {
                        resource.quantity--;
                        break;
                    }
                }
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
    Resource
}