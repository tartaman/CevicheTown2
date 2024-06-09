using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowAndClose : MonoBehaviour
{
    [SerializeField] GameObject objeto;
    [SerializeField] AudioClip openSound;
    [SerializeField] AudioClip closeSound;
    [SerializeField] AudioSource audioSource;
    public void Open()
    {
        audioSource.clip = openSound;
        audioSource.Play();
        objeto.SetActive(true);
        
    }

    public void Close()
    {
        audioSource.clip = closeSound;
        audioSource.Play();
        objeto.SetActive(false);
    }
}
