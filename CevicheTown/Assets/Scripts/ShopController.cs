using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopController : MonoBehaviour
{
    [SerializeField]
    private GameObject shopPanel;
    [SerializeField]
    public TextMeshProUGUI textCurrency;
    public static ShopController instance;
    public float currency;
    void Start()
    {
        instance = this;
        shopPanel.SetActive(false);
        currency = 100;
        textCurrency.text = "" + currency;
    }
    private void Update()
    {
        textCurrency.text = "" + currency;
    }
    public void ToggleShopPanel()
    {
        shopPanel.SetActive(!shopPanel.activeSelf);
        GridBuildingSystem.instance.isPlacing = !shopPanel.activeSelf;
        UpgradeScript.instance.isUpgrading = false;
        UpgradeScript.instance.Upgradecanvasprefab.gameObject.SetActive(false);
        UpgradeScript.instance.selectedBuilding.Selected = false;
    }
}
