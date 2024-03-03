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
        if (Input.GetMouseButtonDown(0) && !GridBuildingSystem.instance.isPlacing && !UpgradeScript.instance.isUpgrading) // Left mouse button
        {
            CheckMouseClick();
        }
        if (selected)
        {
            GetComponentInChildren<SpriteRenderer>().material = selectedMaterial;
        } else if (Placed)
        {
            GetComponentInChildren<SpriteRenderer>().material = selectedMaterialOverride;
        }
        if (buildingType == TypeBuilding.Deco)
        {
            generateDelay = 0;
            generateAmount = 0;
        }
        if (Placed)
        {
            currRange.position = area.position;
            withinRange = GetCoordinatesWithinBounds(currRange);

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
    private List<Vector3Int> GetCoordinatesWithinBounds(BoundsInt bounds)
    {
        List<Vector3Int> coordinates = new List<Vector3Int>();

        foreach (var position in bounds.allPositionsWithin)
        {
            coordinates.Add(position);
        }

        return coordinates;
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

    public void Place()
    {
        Vector3Int positionInt = GridBuildingSystem.instance.gridLayout.LocalToCell(transform.position);
        BoundsInt areaTemp = area;
        areaTemp.position = positionInt;
        Placed = true;
        GridBuildingSystem.instance.TakeArea(areaTemp);
        ShopController.instance.currency -= cost;
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