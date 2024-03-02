using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMan : MonoBehaviour
{

    public void LoadShootingScene() {
        
        SceneManager.LoadScene("LevelShooting");
           
    }

    public void LoadHouseScene()
    {
        SceneManager.LoadScene("LevelHouse");
    }

    public void LoadTeleportLvl()
    {
        SceneManager.LoadScene("LevelTeleport");
    }

}
