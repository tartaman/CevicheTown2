using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UpgradeScript : MonoBehaviour
{
    public static UpgradeScript instance;
    [SerializeField]
    public Building selectedBuilding;
    [SerializeField]
    public Canvas Upgradecanvasprefab;
    [SerializeField]
    public bool isUpgrading;
    public Canvas UpgradeCanvas;
    float UpgradeProductionCost;
    Image imageBuilding;
    public Sprite SpriteBuilding;
    Button upgradeProductionButton;
    // Start is called before the first frame update
    void Start()
    {
        isUpgrading = false;
        instance = this;
        Upgradecanvasprefab.gameObject.SetActive(false);



    }
    // Update is called once per frame
    void Update()
    {
        if (GameObject.FindGameObjectWithTag("ImageBuildingUpgrade") != null)
        {
            imageBuilding = GameObject.FindGameObjectWithTag("ImageBuildingUpgrade").GetComponent<Image>();
            imageBuilding.sprite = selectedBuilding.GetComponentInChildren<SpriteRenderer>().sprite;

        }
        if (isUpgrading)
        {
            UpgradeCanvas.gameObject.SetActive(true);
            selectedBuilding.Selected = true;
        }
        ClickedOtherwhere();

    }
    public void UpgradeProduction() { 
    
        UpgradeProductionCost = Mathf.Ceil(selectedBuilding.cost / 2);
        if (GameObject.FindGameObjectWithTag("ShopManager").GetComponent<ShopController>().currency >= UpgradeProductionCost)
        {
            selectedBuilding.generateAmount += 1;
            GameObject.FindGameObjectWithTag("ShopManager").GetComponent<ShopController>().currency -= UpgradeProductionCost;
            selectedBuilding.cost += UpgradeProductionCost;
        }
    }
    public void DeleteBuilding()
    {
        isUpgrading = false;
        UpgradeCanvas.gameObject.SetActive(false);
        selectedBuilding.Remove();
    }
    public void ClickedOtherwhere()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            if(!EventSystem.current.IsPointerOverGameObject())
            {
                Upgradecanvasprefab.gameObject.SetActive(false);
                isUpgrading = false;
            }
        }
        
    }
}
