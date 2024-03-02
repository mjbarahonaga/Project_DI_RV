using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportPlayer : MonoBehaviour
{
    public GameObject player;
    public GameObject teleport1;
    public GameObject teleport2;
    public GameObject endLvl;


    private void OnTriggerEnter(Collider other)
    {
      
        player.transform.position = new Vector3(teleport2.transform.position.x, 1, teleport2.transform.position.z);
        endLvl.gameObject.SetActive(true);

    }
}
