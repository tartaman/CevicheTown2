using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(JsonReader))]
public class JsonReaderEditor : Editor
{
    public override void OnInspectorGUI()
    {
        JsonReader jsonReader = (JsonReader)target;

        if (GUILayout.Button("Read Json"))
        {
            jsonReader.onClick();
        }

        DrawDefaultInspector();
    }
}
