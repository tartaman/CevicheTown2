using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SaveData))]
public class SaveDataEditor : Editor
{
    public override void OnInspectorGUI()
    {
        SaveData saveData = (SaveData)target;

        if (GUILayout.Button("Save into Json"))
        {
            saveData.SaveIntoJson();
        }

        DrawDefaultInspector(); 
    }
}
