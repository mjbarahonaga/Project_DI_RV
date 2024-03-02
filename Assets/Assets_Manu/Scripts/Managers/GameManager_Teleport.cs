using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;

public class GameManager_Teleport : MonoBehaviour
{
    static GameManager_Teleport _instance;
    public static GameManager_Teleport Instance => _instance;

    [SerializeField] private XROrigin _player;
    public XROrigin GetPlayer => _player;

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(this);
            return;
        }
        _instance = this;
    }
}
