using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class VenderButtonScript : MonoBehaviour
{
    Mission missionAssigned;
    GameObject widget;
    ResourcesDatabase resourcesDatabase;
    Button button;
    MissionsManager missionsManager;
    GameObject itemsDisplay;
    [SerializeField] GameObject itemTextPrefab;

    private void Awake()
    {
        //button = GetComponent<Button>();
    }

    private void Update()
    {
        /*
        if (resourcesDatabase != null && missionAssigned != null)
        {
            
            if (!CompleteMission() && button.interactable)
            {
                
                //button.interactable = false;
            }
            else if (CompleteMission() && !button.interactable)
            {
                button.interactable = true;
            }
            
        }
        */
    }

    public void assignMission(Mission mision, GameObject missionWidget, ResourcesDatabase resources, MissionsManager manager, GameObject itemDisplay)
    {
        missionAssigned = mision;
        widget = missionWidget;    
        resourcesDatabase = resources;
        missionsManager = manager;
        itemsDisplay = itemDisplay;
    }

    public bool CompleteMission()
    {
        foreach (var objecto in missionAssigned.items)
        {
            if(objecto.quantity >= resourcesDatabase.resourcedata[ObjectPosition(objecto.id)].quantity){
                return false;
            }
        }
        return true;
    }

    int ObjectPosition(int id)
    {
        for (int i = 0; i < resourcesDatabase.resourcedata.Count; i++)
        {
            if (resourcesDatabase.resourcedata[i].ID == id)
            {
                return i;
            }
        }
        return -1;
    }

    void RestarObjetosYGanarDinero()
    {
        foreach (var objecto in missionAssigned.items)
        {
            resourcesDatabase.resourcedata[ObjectPosition(objecto.id)].quantity -= objecto.quantity;          
        }
        // ToDo Espacio para ganar dinero cuando victor me diga
    }
    
    public void FinishMission()
    {
        Debug.LogWarning("Eliminando");
        RestarObjetosYGanarDinero();
        missionsManager.GenerateMission();
        Destroy(widget);
        
    }

    public void ShowItems()
    {
        // Eliminar datos que ya estaban en la lista
        
        for (int i = itemsDisplay.transform.GetChild(0).transform.childCount-1; i >= 0; i--)
        {
            Destroy(itemsDisplay.transform.GetChild(0).transform.GetChild(i).gameObject);
        }

        // Ponerle los items a la lista
        
        for (int i = 0; i <  missionAssigned.items.Count; i++)
        {

            GameObject nuevoTexto = Instantiate(itemTextPrefab, itemsDisplay.transform.position, Quaternion.identity, itemsDisplay.transform.GetChild(0).transform);
            // Le ponemos el texto y que se pueda actualizar
            nuevoTexto.transform.GetChild(0).gameObject.AddComponent<UpdateForTextMission>()
                .SetParameters(resourcesDatabase, missionAssigned.items[i].quantity, ObjectPosition(missionAssigned.items[i].id));
            nuevoTexto.transform.GetChild(1).gameObject.GetComponent<Image>().sprite = resourcesDatabase.resourcedata[ObjectPosition(missionAssigned.items[i].id)].sprite;


        }
    }
}
