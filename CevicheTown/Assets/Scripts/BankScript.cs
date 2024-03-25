using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BankScript : Building
{
    // Update is called once per frame
    void Update()
    {
        
    }
    public override void CheckMouseClick()
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
        }
        else if (EventSystem.current.IsPointerOverGameObject())
        {
            selected = true;
        }
        else
        {
            selected = false;
        }
    }
}
