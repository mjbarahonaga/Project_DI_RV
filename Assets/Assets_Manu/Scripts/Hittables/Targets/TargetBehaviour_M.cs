using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;
using System;
using DG.Tweening;

public class TargetBehaviour_M : MonoBehaviour, IHittable_M
{
    public static Action<int> OnScore;
    public static Action<TargetBehaviour_M> OnDeactivatedTarget;
    public int Score = 10;

    //public Animator RefAnimator;


    private CoroutineHandle _coroutine;
    private bool _isActivate = false;

    #region Draw Gizmos
    [Header("Anim positions")]
#if UNITY_EDITOR
    [SerializeField] private Vector3 _sizeCube = Vector3.one;
    [SerializeField] private bool _showGizmos = false;
    [SerializeField] private bool _showInitalPos = true;
    [SerializeField] private Mesh _meshToShow;
    [SerializeField] private bool _copyCurrentPosition = true;
    [SerializeField] private bool _showAnimation = false;
#endif
    public Ease TypeAnimation = Ease.Flash;
    public float FinalPositionY = 0f;
    public float TimeToActivate = 0.4f;
    public float InitalPositionY = 0f;
    public float TimeToDeactivate = 0.2f;

    #endregion

    [Header("Sound")]
    public SoundData ActivatedSound;
    public SoundData DeactivatedSound;
    public SoundData HittedSound;

    //#region Animation Id
    //private int _idAnimActivate;
    //private int _idAnimDeactivate;
    //#endregion

    public void Hit()
    {
        if (!_isActivate) return;
        Timing.KillCoroutines(_coroutine);
        Deactivate();
        OnScore?.Invoke(Score);
    }

    public void RemoveHits()
    {
        var arrows = GetComponentsInChildren<Arrow_M>();
        for (int i = 0; i < arrows.Length; i++)
        {
            arrows[i].gameObject.SetActive(false);
        }
    }

    public void Activate(float timeActive = 4f)   // seconds
    {
        if(_isActivate) return; //Is already active... but it shouldn't, just in case (:
        SoundManager.Instance.PlaySound(transform.position, ActivatedSound);
        transform.DOMoveY(FinalPositionY, TimeToActivate).SetEase(TypeAnimation);
        _coroutine = Timing.RunCoroutine(ActiveCoroutine(timeActive));
        _isActivate = true;
    }

    private IEnumerator<float> ActiveCoroutine(float timeActive)
    {  
        yield return Timing.WaitForSeconds(timeActive);
        Deactivate(hitted:false);
    }

    public async void Deactivate(bool hitted = true)
    {
        Timing.KillCoroutines(_coroutine);
        transform.DOKill();
        _isActivate = false;
        if (hitted)
        {
            SoundManager.Instance.PlaySound(transform.position, HittedSound);
            await transform.DOScale(0.5f, 1).SetEase(Ease.InBounce).AsyncWaitForCompletion();
            SoundManager.Instance.PlaySound(transform.position, DeactivatedSound);
        }
        else
        {
            SoundManager.Instance.PlaySound(transform.position, DeactivatedSound);
        }
        await transform.DOMoveY(InitalPositionY, TimeToDeactivate).SetEase(TypeAnimation).AsyncWaitForCompletion();
        OnDeactivatedTarget?.Invoke(this);
        transform.localScale = Vector3.one;
        RemoveHits();
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (_copyCurrentPosition)
        {
             InitalPositionY = transform.position.y;
             FinalPositionY = transform.position.y;
            _copyCurrentPosition = false;
        }
        if (_showAnimation)
        {
            var sequence = DOTween.Sequence();
            sequence.Append(transform.DOMoveY(FinalPositionY, TimeToActivate).SetEase(TypeAnimation));
            sequence.Append(transform.DOMoveY(InitalPositionY, TimeToDeactivate).SetEase(TypeAnimation)).SetDelay(1f);
            _showAnimation = false;
        }
    }
    private void OnDrawGizmos()
    {

        if (!_showGizmos) return;
        var pos = transform.position;
        if (_meshToShow != null)
        {
            if (_showInitalPos)
            {
                pos.y = InitalPositionY;
                Gizmos.DrawWireMesh(_meshToShow,  pos);
            }
            pos.y = FinalPositionY;
            Gizmos.DrawWireMesh(_meshToShow, pos);
        }
        else
        {
            if (_showInitalPos)
            {
                Gizmos.color = Color.red;
                pos.y = InitalPositionY;
                Gizmos.DrawCube(pos, _sizeCube);
            }
                
            Gizmos.color = Color.green;
            pos.y = FinalPositionY;
            Gizmos.DrawCube(pos, _sizeCube);
        }

    }
#endif
}
