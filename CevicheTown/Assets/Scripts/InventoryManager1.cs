using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class InventoryManager1 : MonoBehaviour
{
    
    public static InventoryManager1 instance;
    [SerializeField]
    public ResourcesDatabase resources;
    [SerializeField]
    GameObject InventoryCanvas;


    [SerializeField]
    GameObject InventoryContent;
    [SerializeField]
    GameObject PrefabToInsert;
    //lista para actualizar y no matar el programa xd
    [SerializeField]
    List<ResourceInInventory> InventoryList = new List<ResourceInInventory>();
    // Start is called before the first frame update
    
    void Start()
    {
        instance = this;
        foreach (var data in resources.resourcedata)
        {
            InventoryList.Add(new ResourceInInventory(Instantiate(PrefabToInsert,InventoryContent.transform), data));
            //Debug.Log("Añadi un resource");
        }
        //InventoryContent.GetComponent<RectTransform>().anchorMin = Vector3.zero;

        InventoryCanvas.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (InventoryCanvas.gameObject.activeSelf)
        {
            Debug.Log("I am updating the inventory menu");
            //Recorro la data para obtener el nombre y la cantidad en tiempo real
            foreach (var data in resources.resourcedata)
            {
                //recorro la lista de los game objects que cree para editarlos
                foreach (var resource in InventoryList)
                {
                    if (resource.Name == data.Name)
                    {
                        //Set the text(Aqui tuve que usar text por que es la referencia al objeto lo que quiero cambiar)
                        resource.Texts[1].text = data.quantity.ToString();
                        resource.Slider.value = data.quantity;
                    }
                }
            }
        }
    }
    public void ToggleInventoryMenu()
    {
       if (InventoryCanvas.gameObject.activeSelf)
        {
            InventoryCanvas.gameObject.SetActive(false);
        } else
        {
            InventoryCanvas.gameObject.SetActive(true);
        }
    }
    




    }


public class ResourceInInventory
{
    GameObject prefab;
    TextMeshProUGUI[] texts;
    Button botonVenta;
    float pricePerUnit;
    Resourcedata data;
    Slider slider;
    public ResourceInInventory(GameObject prefabR, Resourcedata data)
    {
        this.data = data;
        Prefab = prefabR;

        
        //Cambiar el máximo valor del recurso de acuerdo con la capacidad de los inventarios
        Slider slider = prefab.GetComponentInChildren<Slider>();
        this.slider = slider;
        //Debug.Log(slider);
        slider.maxValue = data.MaxQuantity;

        
        // Cambiar el color de la slider al color característico del recurso
        slider.gameObject.transform.GetChild(1).GetChild(0).GetComponent<Image>().color = data.colorRecurso;
        slider.interactable = false;

        slider.maxValue = data.MaxQuantity;
        slider.value = data.quantity;

        //Poner la imagen del recurso
        prefab.transform.GetChild(2).GetComponent<Image>().sprite = data.sprite;
        
        
        //Texts 0 es el nombre y texts 1 es la cantidad
        Texts = prefab.GetComponentsInChildren<TextMeshProUGUI>();
        //Le pongo el texto a las textmeshprougui
        SetDatos();
        Name = Texts[0].text;
        QuantityText = Texts[1].text;
        Quantity = int.Parse(QuantityText);
        PricePerUnit = data.ValuePerUnit;
        //BotonVenta = prefab.GetComponentInChildren<Button>();
        //BotonVenta.onClick.AddListener(SellQuantityPrice);
        
    }

    public GameObject Prefab { get => prefab; set => prefab = value; }
    public TextMeshProUGUI[] Texts { get => texts; set => texts = value; }
    public Button BotonVenta { get => botonVenta; set => botonVenta = value; }
    public float PricePerUnit { get => pricePerUnit; set => pricePerUnit = value; }
    public string Name { get; set; }
    public string QuantityText { get; set; }
    public int Quantity { get; set; }

    public Slider Slider { get => slider; private set => slider = value; }
    public void SellQuantityPrice()
    {
        prefab.GetComponentsInChildren<TextMeshProUGUI>()[1].text = "0";
    }
    public void SetDatos()
    {
        Texts[0].text = data.Name;
        Texts[1].text = data.quantity.ToString();
        //Texts[2].text = data.ValuePerUnit.ToString();
    }
}
