using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TimeManager : MonoBehaviour
{
    public TextMeshProUGUI actualTime;
    public TextMeshProUGUI bestTime;
    public float timer = 0;
    public float recordTime;
    public GameObject endMenu;
    public GameManager gameManager;
    Scene activeScene;


    private void Awake()
    {
        activeScene = SceneManager.GetActiveScene();
        if (activeScene.name == "LevelShooting")
        {
            if (PlayerPrefs.HasKey("saveTime"))
            {
                string timeSaved = PlayerPrefs.GetString("saveTime");
                Debug.Log(timeSaved);
                bestTime.text = timeSaved.ToString();
            }
        }
        if (activeScene.name == "LevelHouse")
        {
            if (PlayerPrefs.HasKey("saveTime2"))
            {
                string timeSaved2 = PlayerPrefs.GetString("saveTime2");
                Debug.Log(timeSaved2);
                bestTime.text = timeSaved2.ToString();
            }
            else
            {
                bestTime.text = recordTime.ToString();
            }
        }
        if (activeScene.name == "LevelTeleport")
        {
            if (PlayerPrefs.HasKey("saveTime3"))
            {
                string timeSaved3 = PlayerPrefs.GetString("saveTime3");
                Debug.Log(timeSaved3);
                bestTime.text = timeSaved3.ToString();
            }
            else
            {
                bestTime.text = recordTime.ToString();
            }
        }

    }
    private void Update()
    {  
        if(gameManager.lvlStarted == true) { 
            timer += Time.deltaTime;
            actualTime.text = timer.ToString("f1");
        }

    }

    public void SaveBestTime()
    {
        if (timer < recordTime) {
        
            recordTime = timer;    
            bestTime.text = recordTime.ToString("f1");
            PlayerPrefs.SetString("saveTime", recordTime.ToString("f1"));
            Debug.Log(PlayerPrefs.GetString("saveTime"));

        }
    }

    public void SaveBestTime2()
    {
        if (timer < recordTime)
        {

            recordTime = timer;
            bestTime.text = recordTime.ToString("f1");
            PlayerPrefs.SetString("saveTime2", recordTime.ToString("f1"));
            Debug.Log(PlayerPrefs.GetString("saveTime2"));

        }
    }
    public void SaveBestTime3()
    {
        if (timer < recordTime)
        {

            recordTime = timer;
            bestTime.text = recordTime.ToString("f1");
            PlayerPrefs.SetString("saveTime3", recordTime.ToString("f1"));
            Debug.Log(PlayerPrefs.GetString("saveTime3"));

        }
    }

}
