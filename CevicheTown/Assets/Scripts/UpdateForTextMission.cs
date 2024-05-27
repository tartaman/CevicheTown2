using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpdateForTextMission : MonoBehaviour
{
    ResourcesDatabase database;
    int objective;
    int indexObjectDatabase;
    TextMeshProUGUI textComponent;

    private void Awake()
    {
        textComponent = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        if(database != null)
        {
            Debug.LogWarning(indexObjectDatabase);

            textComponent.text = $"{database.resourcedata[indexObjectDatabase].quantity}/{objective}";
        }
         
    }

    public void SetParameters(ResourcesDatabase resources, int missionObjective, int indexOfResourseInDatabase)
    {
        database = resources;
        objective = missionObjective;
        indexObjectDatabase = indexOfResourseInDatabase;
    }
}
