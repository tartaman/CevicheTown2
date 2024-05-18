using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.UI;

public class VenderButtonScript : MonoBehaviour
{
    Mission missionAssigned;
    GameObject widget;
    ResourcesDatabase resourcesDatabase;
    Button button;
    MissionsManager missionsManager;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    private void Update()
    {
        
        if (resourcesDatabase != null)
        {
            if (!CompleteMission() && button.interactable)
            {
                button.interactable = false;
            }
            else if (CompleteMission() && !button.interactable)
            {
                button.interactable = true;
            }

        }
        
    }

    public void assignMission(Mission mision, GameObject missionWidget, ResourcesDatabase resources, MissionsManager manager)
    {
        missionAssigned = mision;
        widget = missionWidget;    
        resourcesDatabase = resources;
        missionsManager = manager;
    }

    public bool CompleteMission()
    {
        foreach (var objecto in missionAssigned.items)
        {
            if(objecto.Item2 >= resourcesDatabase.resourcedata[ObjectPosition(objecto.Item1)].quantity){
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
            resourcesDatabase.resourcedata[ObjectPosition(objecto.Item1)].quantity -= objecto.Item2;          
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
}
