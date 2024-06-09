using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class MissionsManager : MonoBehaviour
{
    [SerializeField] public MissionProgress missionProgress;
    [SerializeField] private MissionsDatabase missionsDatabase;
    [SerializeField] private ResourcesDatabase resourcesDatabase;
    [SerializeField] private GameObject missionWidget;
    [SerializeField] private GameObject panel;
    [SerializeField] private GameObject ItemDisplay;
    [SerializeField] private Button buttonVender;
    [SerializeField] private TextMeshProUGUI textoRecompensa;


    private void Start()
    {
        
        if(missionsDatabase.missions.Count < missionProgress.missionsInfo.maxQuantityOfMissions)
        {
            int misionesFaltantes = missionProgress.missionsInfo.maxQuantityOfMissions - missionsDatabase.missions.Count;
            for (int i = 0; i < misionesFaltantes; i++)
            {
                Mission msionsita = GenerateMission();
                SetVisualMission(msionsita);
            }
            
        }
       
        
        LoadMissions();

    }

    public Mission GenerateMission()
    {
        Mission newMission = new Mission();
        int level = missionProgress.missionsInfo.level - 1;
        int random = Random.Range(1, missionProgress.missionsInfo.maxQuantityOfItems + 1);

        for (int i = 0; i < random; i++)
        {
            
            int itemId = Random.Range(0, resourcesDatabase.resourcedata.Count);
            int itemQuantity = Random.Range(missionProgress.missionsInfo.minOfItemsPerLevel[level], missionProgress.missionsInfo.maxOfItemsPerLevel[level] + 1);

            (bool, int) result = newMission.FindIfAlreadyAdded(itemId);
            if (!result.Item1)
                newMission.items.Add(new Item(itemId, itemQuantity));
            else
            {
                newMission.items[result.Item2] =  new Item(itemId, newMission.items[result.Item2].quantity + itemQuantity);
            }

        }

        // Calcular la recompensa, que va del 70 al 125% del valor total de los items
        float recompensa = Random.Range(0.70f, 1.25f);

        //Calcular la suma de la recompensa
        float suma = 0;
        foreach (var item in newMission.items)
        {
            float precio = 0;
            foreach (var producto in resourcesDatabase.resourcedata)
            {

                if (producto.ID == item.id)
                {
                    precio = producto.ValuePerUnit;
                }
            }
            suma += item.quantity * precio;
        }
        suma *= recompensa;
        newMission.reward = (int)suma;
        SaveMissionInDB(newMission);
        return newMission;
        
    }

    void SaveMissionInDB(Mission mission)
    {

        missionsDatabase.missions.Add(mission);
    }

    public void DeleteMissionInDB(Mission mission)
    {
        // Solo elimina la primera instancia de esa mision
        foreach(Mission quest in missionsDatabase.missions) { 
            if(quest == mission)
            {
                missionsDatabase.missions.Remove(quest);
                return;
            }
        }
    }
    void SetVisualMission(Mission newMission)
    {
        GameObject mission = Instantiate(missionWidget, transform.position, Quaternion.identity, panel.transform);
        mission.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = $"${newMission.reward}";
        mission.GetComponent<VenderButtonScript>().assignMission(newMission, mission.gameObject, resourcesDatabase, this, ItemDisplay, buttonVender, textoRecompensa);
        mission.GetComponent<Button>().onClick.AddListener(mission.GetComponent<VenderButtonScript>().ShowItems);
    }


    void LoadMissions()
    {
        foreach(Mission missionInDB in missionsDatabase.missions)
        {
           
            SetVisualMission(missionInDB);
        }
    }

    public Mission GenerateAndDeleteForVisual(Mission misionAntigua)
    {
        DeleteMissionInDB(misionAntigua);
        Mission misionNueva = GenerateMission();
        SetVisualMission(misionNueva);
        panel.transform.GetChild(0).GetComponent<VenderButtonScript>().ShowItems();
        return misionNueva;
    }
}
