using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RadioTurn : MonoBehaviour
{
    //Asignamos variables al material y audio
    public Material radioMaterial;
    public AudioSource audioSource;
    public GameObject endLevel;

    //Seteamos el material inicial a ngero y el volumen bajo
    public void Start()
    {
        radioMaterial.color = Color.black;
        audioSource.volume = 0.005f;
    }

    //metodo para cambiar el color de la radio al interactuar con Poke
    public void turnRadioGreen()
    {
        if(radioMaterial.color != Color.black)
        {
            radioMaterial.color = Color.black;
            audioSource.Stop();
        }
        else
        {
            radioMaterial.color = Color.green;
            audioSource.Play();
        }
    }

    //metodos para subir y bajar el volumen
    public void UpVolume()
    {
        audioSource.volume = 1f;
        endLevel.SetActive(true);
    }

    public void DownVolume()
    {
        audioSource.volume = 0.01f;
    }
}
