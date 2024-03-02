using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpdateButtons : MonoBehaviour
{
    [SerializeField]
    Canvas UpgradeCanvas;
    [SerializeField]
    TextMeshProUGUI TextProduction;
    [SerializeField]
    UpgradeScript upgrades;
    float UpgradeProductionCost;
    private void Start()
    {
        upgrades = GetComponent<UpgradeScript>();
    }
    // Update is called once per frame
    void Update()
    {
        //Calcular costo mejoras
        if (upgrades.selectedBuilding != null)
        {
            UpgradeProductionCost = Mathf.Ceil(upgrades.selectedBuilding.cost / 2);
        }
        //Actualizar el boton
        if (UpgradeCanvas != null)
        {
            TextProduction.text = $"Upgrade Production \n${UpgradeProductionCost}";
        }
    }
}
