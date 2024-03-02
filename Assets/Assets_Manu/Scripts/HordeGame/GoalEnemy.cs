using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class GoalEnemy : MonoBehaviour
{
    private void Awake()
    {
        if(TryGetComponent(out BoxCollider collider))
        {
            collider.isTrigger = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out EnemyBehaviour enemy))
        {
            enemy.Deactivate();
            GameManager_Horde.Instance.ReduceLives();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out EnemyBehaviour enemy))
        {
            enemy.Deactivate();
            GameManager_Horde.Instance.ReduceLives();
        }
    }

}
