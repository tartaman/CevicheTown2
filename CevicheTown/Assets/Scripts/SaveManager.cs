using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    [SerializeField]
    Canvas savecanvas;
    [SerializeField]
    SaveData saver;

    public void togglecanvas()
    {
        if (savecanvas != null)
        {
            savecanvas.gameObject.SetActive(!savecanvas.gameObject.activeSelf);
        }
    }
    public void save()
    {
        saver.SaveIntoJson();
        togglecanvas();
    }
}
