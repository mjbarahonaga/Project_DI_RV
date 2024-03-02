using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{

    //Riffle bullet prefab
    public GameObject riffleBullet;

    //Bullet strating point
    public Transform riffleBulletStart;

    //Bullet speed
    public float bulletSpeed =1500f;

    public void FireRiffle()
    {
        
        GameObject newBullet = Instantiate(riffleBullet, riffleBulletStart.position, riffleBulletStart.rotation);
        newBullet.GetComponent<Rigidbody>().AddForce(riffleBulletStart.up *  bulletSpeed);

        Destroy(newBullet, 2);
    }

}
