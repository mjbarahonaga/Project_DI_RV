/// Tutorial from Unity: Creating bow and arrow gameplay in VR!

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class PullInteraction_M : XRBaseInteractable
{
    private Bow_M _bow;

    public float PullAmount { get; private set; } = 0f;
    public Transform Start , End ;
    private XRController PullingController = null;
    private IXRSelectInteractor PullingInteractor = null;

    [Header("Polish")]
    public LineRenderer StringLine;
    [ColorUsage(true, true)]
    public Color StringNormalCol, StringPulledCol;
    public ParticleSystem LineParticle;
    public bool ShowHandsOnPull = true;
    public GameObject RightHand, LeftHand;

    protected override void Awake()
    {
        base.Awake();
        _bow = GetComponentInParent<Bow_M>();
    }

    public void ForceInteract(IXRSelectInteractor interactor)
    {
        interactionManager.SelectEnter(interactor, this);
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);
        if(args.interactorObject is XRBaseControllerInteractor controllerInteractor && controllerInteractor != null)
        {
            PullingController = controllerInteractor.gameObject.GetComponent<XRController>();
            PullingInteractor = args.interactorObject;
            if(PullingController != null)
            {
                HapticManager_M.Impulse(.5f, .05f, PullingController.inputDevice);
            }
        }
        
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);
        PullingController = null;
        PullingInteractor = null;
        PullAmount = 0;

        StringLine?.material.SetColor("_EmissionColor", StringNormalCol);
        LineParticle?.Play();
    }

    public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
    {
        base.ProcessInteractable(updatePhase);
        if(updatePhase == XRInteractionUpdateOrder.UpdatePhase.Dynamic)
        {
            if(isSelected)
            {
                Vector3 pullPosition = PullingInteractor.transform.position;
                PullAmount = CalculatePull(pullPosition);

                if (PullingController && PullAmount > .3)
                    HapticManager_M.Impulse(PullAmount / 5f, .05f, PullingController.inputDevice);

                StringLine?.material.SetColor("_EmissionColor",
                    Color.Lerp(StringNormalCol, StringPulledCol, PullAmount));
            }
        }
    }

    private float CalculatePull(Vector3 pullPosition) 
    {
        Vector3 pullDirection = pullPosition - Start.position;
        Vector3 targetDirection = End.position - Start.position;
        float maxLength = targetDirection.magnitude;

        targetDirection.Normalize();
        float pullValue = Vector3.Dot(pullDirection, targetDirection) / maxLength;
        return Mathf.Clamp(pullValue, 0, 1);
    }

    public void ShowHand(bool show)
    {
        if(show)
        {
            if(ShowHandsOnPull && _bow.HandGrabbed != HandType.None)
            {
                LeftHand.SetActive(HandType.Left == _bow.HandGrabbed);
                RightHand.SetActive(HandType.Right == _bow.HandGrabbed);
            } 
        }
        else
        {
            if (ShowHandsOnPull)
            {
                LeftHand.SetActive(false);
                RightHand.SetActive(false);
            }
        }
    }
}
