using UnityEngine;

[CreateAssetMenu(fileName = "SoundData.asset", menuName = "SoundData/Create Scriptable Sound")]
public class SoundData : ScriptableObject
{
    public AudioClip Clip;
    public float Pitch;
    public float Volume;
    public bool Loop = false;
}
