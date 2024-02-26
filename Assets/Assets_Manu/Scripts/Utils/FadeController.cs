using MEC;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeController : MonoBehaviour
{
    private static FadeController _instance;
    public static FadeController Instance => _instance;

    public CanvasGroup CanvasToFade;
    private CoroutineHandle _coroutineFade;

    private float _lerpFadeValue = 0f;
    private float _timeElapsedFading = 0f;


    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(this);
            return;
        }
        _instance = this;
    }

    private IEnumerator<float> FadeIn(float timeToFade, Action function, bool afterComplete)
    {
        _lerpFadeValue = CanvasToFade.alpha;
        _timeElapsedFading = Mathf.Lerp(0, 1, _lerpFadeValue);
        var timeTotal = 1f / timeToFade;
        while (_lerpFadeValue < 1f)
        {
            _lerpFadeValue = Mathf.Lerp(0, 1, timeTotal * _timeElapsedFading);
            _timeElapsedFading += Time.deltaTime;
            CanvasToFade.alpha = _lerpFadeValue;
            yield return Timing.WaitForOneFrame;
        }

        if (afterComplete)
            function?.Invoke();

        yield return 0f;
    }

    private IEnumerator<float> FadeOut(float timeToReturn, Action function, bool afterComplete)
    {
        _lerpFadeValue = CanvasToFade.alpha;
        _timeElapsedFading = Mathf.Lerp(1, 0, _lerpFadeValue);
        var timeTotal = 1f / timeToReturn;
        while (_lerpFadeValue > 0f)
        {
            _lerpFadeValue = Mathf.Lerp(1, 0, timeTotal * _timeElapsedFading);
            _timeElapsedFading += Time.deltaTime;
            CanvasToFade.alpha = _lerpFadeValue;
            yield return Timing.WaitForOneFrame;
        }

        if (afterComplete)
            function?.Invoke();

        yield return 0f;
    }

    public void StartFadeIn(float timeToFade = 1f, Action function = null, bool afterComplete = false)
    {
        Timing.KillCoroutines(_coroutineFade);
        _coroutineFade = Timing.RunCoroutine(FadeIn(timeToFade, function, afterComplete));
        if (!afterComplete)
            function?.Invoke();
    }

    public void StartFadeOut(float timeToReturn = 1f, Action function = null, bool afterComplete = false)
    {
        Timing.KillCoroutines(_coroutineFade);
        _coroutineFade = Timing.RunCoroutine(FadeOut(timeToReturn, function, afterComplete));
        if (!afterComplete)
            function?.Invoke();
    }
}
