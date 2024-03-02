using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool lvlStarted;

    public void StartLVL()
    {
        lvlStarted = true;
    }

    public void FinishLVL()
    {
        lvlStarted= false;
    }


}
