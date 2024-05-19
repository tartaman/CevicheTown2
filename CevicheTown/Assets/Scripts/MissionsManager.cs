using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.UI;
using static UnityEditor.Progress;


public class MissionsManager : MonoBehaviour
{
    [SerializeField] private MissionProgress missionProgress;
    [SerializeField] private MissionsDatabase missionsDatabase;
    [SerializeField] private ResourcesDatabase resourcesDatabase;
    [SerializeField] private GameObject missionWidget;
    [SerializeField] private GameObject panel;
    [SerializeField] private GameObject buttonPrefab;


    private void Start()
    {
        LoadMissions();

        /*
        for(int i = 0; i < missionProgress.missionsInfo.maxQuantityOfMissions; i++)
        {
            Mission msionsita= GenerateMission();
            SetVisualMission(msionsita);
        }
        */
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
        // Ponerle los items a la lista
        GameObject mission = Instantiate(missionWidget, transform.position, Quaternion.identity, panel.transform);
        for (int i = 0; i < newMission.items.Count; i++)
        {
            GameObject missionText = new GameObject();
            TextMeshProUGUI text = missionText.AddComponent<TextMeshProUGUI>();
            text.fontSize = 70;
            text.alignment = TextAlignmentOptions.Center;
            text.enableWordWrapping = false;
            string objectName = "";
            foreach (var item in resourcesDatabase.resourcedata)
            {
                if (item.ID == newMission.items[i].id)
                {
                    objectName = item.Name;
                    break;
                }
            }

            text.text = $"{objectName}...x{newMission.items[i].quantity}";

            Instantiate(text, transform.position, Quaternion.identity, mission.transform);
        }

        GameObject recompensaText = new GameObject();
        TextMeshProUGUI texto = recompensaText.AddComponent<TextMeshProUGUI>();
        texto.text = $"${newMission.reward}";
        texto.fontSize = 70;
        texto.alignment = TextAlignmentOptions.Center;
        texto.enableWordWrapping = false;
        Instantiate(texto, transform.position, Quaternion.identity, mission.transform);

        GameObject button = Instantiate(buttonPrefab, transform.position, Quaternion.identity, mission.transform);
        button.AddComponent<VenderButtonScript>().assignMission(newMission, mission.gameObject, resourcesDatabase, this);
        button.GetComponent<Button>().onClick.AddListener(button.GetComponent<VenderButtonScript>().FinishMission);
    }

    void LoadMissions()
    {
        foreach(Mission missionInDB in missionsDatabase.missions)
        {
            Debug.LogWarning(missionInDB.items.Count);
            SetVisualMission(missionInDB);
        }
    }

}
