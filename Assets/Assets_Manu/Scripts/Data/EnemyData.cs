using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData.asset", menuName = "EnemyData/Create Scriptable Enemy")]
public class EnemyData : ScriptableObject
{
    public int Lives = 1;
    public float Speed = 2f;
    public int Reward = 5;
}
