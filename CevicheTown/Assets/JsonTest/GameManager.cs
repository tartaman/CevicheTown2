using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            Debug.LogWarning("Saving grid into json");
            this.GetComponent<SaveData>().SaveIntoJson();
        }
    }
}
