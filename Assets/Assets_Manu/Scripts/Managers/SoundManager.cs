using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using MEC;

public class SoundManager : MonoBehaviour
{
    static SoundManager _instance;
    public static SoundManager Instance => _instance;

    public int SourceCount;
    public AudioSource RefAudioSource;
    private IObjectPool<AudioSource> _audioSourcePool;

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(this);
            return;
        }
        _instance = this;

        
    }

    private void Start()
    {
        Init();
    }

    public void Init()
    {
        _audioSourcePool = new ObjectPool<AudioSource>(
            createFunc: () =>
            {
                var obj = Instantiate(RefAudioSource, transform).GetComponent<AudioSource>();
                obj.enabled = false;
                return obj;
            },
            actionOnGet: (source) => source.enabled = true,
            actionOnRelease: (source) => source.enabled = false,
            actionOnDestroy: (source) => Destroy(source.gameObject),
            collectionCheck: false,
            defaultCapacity: SourceCount,
            maxSize: SourceCount * 2);

        // Instantiate them all at once at the beginning
        List<AudioSource> temp = new List<AudioSource>();
        for (int i = 0; i < SourceCount; ++i)
        {
            temp.Add(_audioSourcePool.Get());
        }

        for (int i = 0; i < SourceCount; ++i)
        {
            _audioSourcePool.Release(temp[i]);
        }
    }

    public void PlaySound(Vector3 worldPos, SoundData data)
    {
        var source = _audioSourcePool.Get();
        source.transform.position = worldPos;
        source.clip = data.Clip;
        source.volume = data.Volume;
        source.pitch = data.Pitch;
        source.loop = false; //data.Loop;
        source.Play();
        
        if (data.Loop)
        {
            // TODO: 
        }
        else
        {
            Timing.RunCoroutine(ReturnToPoolAfterPlay(source), Segment.SlowUpdate);
        }
    }

    public void PlaySound(Vector3 worldPos, AudioClip clip, float volume, float pitch, bool loop = false)
    {
        var source = _audioSourcePool.Get();
        source.transform.position = worldPos;
        source.clip = clip;
        source.volume = volume;
        source.pitch = pitch;
        source.loop = false; //loop;
        source.Play();

        if (loop)
        {
            // TODO: 
        }
        else
        {
            Timing.RunCoroutine(ReturnToPoolAfterPlay(source), Segment.SlowUpdate);
        }
    }

    private IEnumerator<float> ReturnToPoolAfterPlay(AudioSource source)
    {
        yield return Timing.WaitForSeconds(source.clip.length);
        _audioSourcePool.Release(source);
    }
}
