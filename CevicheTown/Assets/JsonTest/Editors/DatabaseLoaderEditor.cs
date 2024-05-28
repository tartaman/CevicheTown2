using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DatabaseLoader))]
public class DatabaseLoaderEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DatabaseLoader dbLoader = (DatabaseLoader)target;

        if (GUILayout.Button("Login"))
        {
            dbLoader.StartCoroutine(dbLoader.getUserData(dbLoader.username, dbLoader.password));
        }

        if (GUILayout.Button("Sign Up"))
        {
            dbLoader.StartCoroutine(dbLoader.signUpUser(dbLoader.username, dbLoader.password));
        }

        if (GUILayout.Button("Save Game"))
        {
            dbLoader.StartCoroutine(dbLoader.saveGame(dbLoader.username, dbLoader.fileName, dbLoader.fileContents));
        }

        if(GUILayout.Button("Load Game"))
        {
            dbLoader.StartCoroutine(dbLoader.loadGame(dbLoader.username, dbLoader.fileName));
        }

        DrawDefaultInspector();
    }
}
