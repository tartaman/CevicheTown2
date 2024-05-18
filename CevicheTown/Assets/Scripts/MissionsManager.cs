using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
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


    private void Awake()
    {
        for (int i = 0; i < 6; i++)
        {
            GenerateMission();
        }
    }

    private void GenerateMission()
    {
        Mission newMission = new Mission();
        int level = missionProgress.missionsInfo.level - 1;
        int random = Random.Range(1, missionProgress.missionsInfo.maxQuantityOfItems + 1);
        for (int i = 0; i < random; i++)
        {
            if (i > resourcesDatabase.resourcedata.Count) {
                return;
            }
            int itemId = Random.Range(0, resourcesDatabase.resourcedata.Count);
            int itemQuantity = Random.Range(missionProgress.missionsInfo.minOfItemsPerLevel[level], missionProgress.missionsInfo.maxOfItemsPerLevel[level] + 1);

            bool alreadyAdded = false;

            if (!alreadyAdded)
                newMission.items.Add((itemId, itemQuantity));

        }

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
                if (item.ID == newMission.items[i].Item1)
                {
                    objectName = item.Name;
                    break;
                }
            }

            text.text = $"{objectName}...x{newMission.items[i].Item2}";

            Instantiate(text, transform.position, Quaternion.identity, mission.transform);
        }

        // Calcular la recompensa, que va del 70 al 100% del valor total de los items
        float recompensa = Random.Range(0.70f, 1.25f);

        //Calcular la suma
        float suma = 0;
        foreach (var item in newMission.items)
        {
            float precio = 0;
            foreach (var producto in resourcesDatabase.resourcedata)
            {

                if (producto.ID == item.Item1)
                {
                    precio = producto.ValuePerUnit;
                }
            }
            suma += item.Item2 * precio;
        }
        suma *= recompensa;

        GameObject recompensaText = new GameObject();
        TextMeshProUGUI texto = recompensaText.AddComponent<TextMeshProUGUI>();
        texto.text = $"${(int)suma}";
        texto.fontSize = 70;
        texto.alignment = TextAlignmentOptions.Center;
        texto.enableWordWrapping = false;
        Instantiate(texto, transform.position, Quaternion.identity, mission.transform);

        GameObject button = Instantiate(buttonPrefab, transform.position, Quaternion.identity, mission.transform);
        button.AddComponent<VenderButtonScript>().assignMission(newMission, mission, resourcesDatabase);
        button.GetComponent<Button>().onClick.AddListener(button.AddComponent<VenderButtonScript>().FinishMission);
    }
}
