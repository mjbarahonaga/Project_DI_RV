using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxHealth : MonoBehaviour
{
    //Declaramos las variables de vida
    [SerializeField] float health, maxHealth = 100;

    //Asignamos el GO de la llave
    public GameObject key;

    //Al inico la vida es máxima
    private void Start()
    {
        health = maxHealth;
    }

    //Cuando el script de la bala haga collider se llamara la funcion de recibir daño de la caja
    public void TakeDamage(float damage)
    {
        health -= damage;

        //Cuando la vida llega al minimo el objeto de la caja desapreece y activamos la llave
        if(health <= 0)
        {
            Destroy(gameObject);
            key.SetActive(true);
        }
    }
}
