using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;
using System;

public class TargetBehaviour_M : MonoBehaviour, IHittable_M
{
    public static Action<int> OnScore;
    public static Action<TargetBehaviour_M> OnDeactivatedTarget;
    public int Score = 10;

    public Animator RefAnimator;


    private CoroutineHandle _coroutine;
    private bool _isActivate = false;

    #region Animation Id
    private int _idAnimActivate;
    private int _idAnimDeactivate;
    #endregion

    public void Hit()
    {
        if (!_isActivate) return;
        Timing.KillCoroutines(_coroutine);
        Deactivate();
        OnScore?.Invoke(Score);
    }

    private void Awake()
    {
        _idAnimActivate = Animator.StringToHash("Activate");
        _idAnimDeactivate = Animator.StringToHash("Deactivate");
    }

    public void Activate(float timeActive = 4f)   // seconds
    {
        if(_isActivate) return; //Is already active... but it shouldn't, just in case (:
        RefAnimator.SetTrigger(_idAnimActivate);
        _coroutine = Timing.RunCoroutine(ActiveCoroutine(timeActive));
        _isActivate = true;
    }

    private IEnumerator<float> ActiveCoroutine(float timeActive)
    {  
        yield return Timing.WaitForSeconds(timeActive);
        Deactivate();
    }

    public void Deactivate()
    {
        RefAnimator.SetTrigger(_idAnimDeactivate);
        _isActivate = false;
    }
}
