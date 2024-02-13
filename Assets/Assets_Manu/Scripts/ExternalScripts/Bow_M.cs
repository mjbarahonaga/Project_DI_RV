/// Tutorial from Unity: Creating bow and arrow gameplay in VR!

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Bow_M : XRGrabInteractable
{
    public Transform Notch;
    private PullInteraction_M _pullInteraction;
    private LineRenderer _lineRenderer;

    [Header("Sound")]
    public AudioClip GrabClip;

    [Header("Quiver")]
    public Transform Quiver;
    public Vector2 QuiverOffset;

    private HandType _handGrabbed = HandType.None;
    public HandType HandGrabbed => _handGrabbed;

    protected override void Awake()
    {
        base.Awake();
        _pullInteraction = GetComponentInChildren<PullInteraction_M>();
        _lineRenderer = GetComponentInChildren<LineRenderer>();
    }

    public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
    {
        base.ProcessInteractable(updatePhase);
        if(updatePhase == XRInteractionUpdateOrder.UpdatePhase.Dynamic)
        {
            if(isSelected)
            {
                UpdateBow(_pullInteraction.PullAmount);
            }
        }
    }

    private void UpdateBow(float value)
    {
        Vector3 linePosition = Vector3.forward * Mathf.Lerp(-0.25f, -0.5f, value);
        Notch.localPosition = linePosition;
        _lineRenderer.SetPosition(1,linePosition);
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);
    }

    public void OffsetQuiver(SelectEnterEventArgs args)
    {
        if(args.interactorObject is XRBaseControllerInteractor controllerInteractor && controllerInteractor != null)
        {
            var hand = controllerInteractor.gameObject.GetComponentInChildren<GameplayHand>();
            _handGrabbed = hand.HandType;
            bool right = hand.HandType == HandType.Right;
            Quiver.localPosition = new Vector3(QuiverOffset.x * (right ? -1 : 1), QuiverOffset.y, Quiver.localPosition.z);
        }
    }
}

