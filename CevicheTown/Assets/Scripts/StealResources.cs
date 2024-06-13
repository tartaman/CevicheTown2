using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StealResources : MonoBehaviour
{
    [SerializeField] ResourcesDatabase resources;
    [SerializeField] float secondsToSteal;
    [SerializeField] float probabilityToSteal;
    [SerializeField] float maximumPercentageToSteal;
    [SerializeField] float minimumPercentageToSteal;
    [SerializeField] GameObject ventana;
    bool canSteal;

    private void Awake()
    {
        canSteal = false;
        StartCoroutine(Wait(120));
    }

    void Update()
    {
        if(canSteal)
        {
            float probabilidad = Random.Range(0.01f, 1);
            if(probabilidad <= probabilityToSteal)
            {
                Steal();
            }
            StartCoroutine(Wait(secondsToSteal));
        }
    }

    void Steal()
    {
        if (canSteal)
        {
            canSteal = false;
            int indiceElegido = Random.Range(0, resources.resourcedata.Count);
            int cantidadEliminada = (int)(Random.Range(minimumPercentageToSteal, maximumPercentageToSteal) * resources.resourcedata[indiceElegido].quantity);
            if(resources.resourcedata[indiceElegido].quantity - cantidadEliminada < 0)
            {
                resources.resourcedata[indiceElegido].quantity = 0;
            }
            else
            {
                resources.resourcedata[indiceElegido].quantity -= cantidadEliminada;
            }
            
            ventana.SetActive(true);
            ventana.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = $"Han robado x{cantidadEliminada} de {resources.resourcedata[indiceElegido].Name}.";
            
        }
    }

    IEnumerator Wait(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        canSteal = true;
    }
}
