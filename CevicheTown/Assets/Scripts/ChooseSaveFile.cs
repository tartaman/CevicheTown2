using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class ChooseSaveFile : MonoBehaviour
{
    [SerializeField]
    DatabaseLoader databaseloader;
    [SerializeField]
    Canvas slotcanvas;
    [SerializeField]
    LoadExistingGame loader;
    public void chooseSaveFile(string saveName)
    {
        databaseloader.fileName = saveName;
        slotcanvas.gameObject.SetActive(false);
        loader.LoadGame();
    }
}
