using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowAndClose : MonoBehaviour
{
    [SerializeField] GameObject objeto;
    public void Open()
    {
        objeto.SetActive(true);
    }

    public void Close()
    {
        objeto.SetActive(false);
    }
}
