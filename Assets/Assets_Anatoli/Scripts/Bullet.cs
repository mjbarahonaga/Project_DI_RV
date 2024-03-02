using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    //Asignamos GO de la caja
    GameObject box;

    //en colision con cualquier collider se destruye la bala 
    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);

        //BSi el objeto que collisiona es la caja se llamam la funcion de recibir daño

        if(collision.gameObject.TryGetComponent<BoxHealth>(out BoxHealth boxObject))
        {
            //Pasamos por parámetro que el daño recibido sea de 1 unidad
            boxObject.TakeDamage(1);
            boxObject.GetComponent<AudioSource>().Play();
        }

    }
}
