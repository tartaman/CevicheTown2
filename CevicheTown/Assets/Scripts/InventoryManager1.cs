using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager1 : MonoBehaviour
{
    
    public static InventoryManager1 instance;
    [SerializeField]
    public ResourcesDatabase resources;
    [SerializeField]
    Canvas InventoryCanvas;
    [SerializeField]
    GameObject InventoryContent;
    [SerializeField]
    GameObject PrefabToInsert;
    //lista para actualizar y no matar el programa xd
    List<GameObject> InventoryList = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        foreach (var data in resources.resourcedata)
        {
            GameObject ToInsert = Instantiate(PrefabToInsert);
            TextMeshProUGUI[] texts = ToInsert.GetComponentsInChildren<TextMeshProUGUI>();
            // Nombre
            texts[0].text = data.Name;
            texts[1].text = data.quantity.ToString();
            InventoryList.Add(ToInsert);
            ToInsert.transform.SetParent(InventoryContent.transform,false);
        }
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
                foreach (var GO in InventoryList)
                {
                    //Obtengoi el texto en ñps game objects
                    TextMeshProUGUI[] texts = GO.GetComponentsInChildren<TextMeshProUGUI>();
                    if (texts[0].text == data.Name)
                    {
                        //Set the text
                        texts[1].text = data.quantity.ToString();
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
