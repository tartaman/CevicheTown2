using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopController : MonoBehaviour
{
    [SerializeField]
    private GameObject shopPanel;
    [SerializeField]
    public TextMeshProUGUI textCurrency;
    public static ShopController instance;
    public float currency;

    [SerializeField]
    public ObjectsDatabase Buildings;
    [SerializeField]
    Canvas ShopCanvas;
    [SerializeField]
    GameObject ShopContent;
    [SerializeField]
    GameObject PrefabToInsert;
    [SerializeField]
    List<GameObject> ShopList = new List<GameObject>();

    //Agregu� esto lit nada mas para ponerlo en 1 al principio
    [SerializeField] Scrollbar scrollbar;

    // Y esto para guardar el dinero
    [SerializeField] public MissionProgress missionProgress;

    // Para mostrar el dinero en la tienda
    [SerializeField] TextMeshProUGUI textTienda;
    void Start()
    {
        
        instance = this;
        shopPanel.SetActive(false);
        currency = missionProgress.money;
        textCurrency.text = "" + currency;
        foreach (var data in Buildings.objectsdata)
        {
            GameObject ToInsert = Instantiate(PrefabToInsert, ShopContent.transform);
            Button placeButton = ToInsert.GetComponentInChildren<Button>();
            placeButton.onClick.AddListener(() => GridBuildingSystem.instance.InitializeWithBuilding(data.prefab));
            placeButton.onClick.AddListener(ToggleShopPanel);
            CheckPrice checador = ToInsert.GetComponentInChildren<CheckPrice>();
            checador.CurrencyText = textCurrency;
            checador.SelledBuild = data.prefab;
            ToInsert.GetComponent<Image>().sprite = data.prefab.GetComponentInChildren<SpriteRenderer>().sprite;
            ShopList.Add(ToInsert);
        }
        scrollbar.value = 2;
    }
    private void Update()
    {
        currency = missionProgress.money;
        textCurrency.text = "" + currency;
        textTienda.text = $"${missionProgress.money}";
    }
    public void ToggleShopPanel()
    {
        shopPanel.SetActive(!shopPanel.activeSelf);
        GridBuildingSystem.instance.isPlacing = !shopPanel.activeSelf;
        UpgradeScript.instance.isUpgrading = false;
        UpgradeScript.instance.Upgradecanvasprefab.gameObject.SetActive(false);
        if (UpgradeScript.instance.selectedBuilding != null)
            UpgradeScript.instance.selectedBuilding.Selected = false;
    }
}
