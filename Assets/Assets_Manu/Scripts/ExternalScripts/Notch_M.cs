/// Tutorial from Unity: Creating bow and arrow gameplay in VR!

using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class Notch_M : XRSocketInteractor
{
    private PullInteraction_M _pullInteraction;
    private Arrow_M _currentArrow;
    private Arrow_M _prevArrow = null;
    [Header("Sound")]
    public AudioClip AttachClip;

    protected override void Awake()
    {
        base.Awake();
        _pullInteraction = GetComponent<PullInteraction_M>();
    }

    //protected override void OnEnable()
    //{
    //    base.OnEnable();
    //    //_pullInteraction.selectExited. += TryToReleaseArrow;
    //}


    //protected override void OnDisable()
    //{
    //    base.OnEnable();
    //    //_pullInteraction.selectExited.RemoveListener(TryToReleaseArrow);
    //}

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        
        base.OnSelectEntered(args);
        var arrow = args.interactableObject as Arrow_M;
        if (arrow == _prevArrow) return;
        if (arrow == null) return;

        StoreArrow(arrow);
    }

    protected override void OnHoverEntered(HoverEnterEventArgs args)
    {
        base.OnHoverEntered(args);
        if(args.interactableObject is Arrow_M arrow  && arrow.firstInteractorSelecting is HandInteractor_M hand)
        {
            arrow.interactionManager.SelectExit(hand,(IXRSelectInteractable)arrow);
            hand.ForceDeinteract();
            _pullInteraction.ForceInteract(hand);
            hand.ForceInteract(_pullInteraction);

            NotchSounds(AttachClip, 3, 3.3f, 5);
        }
    }

    private void StoreArrow(Arrow_M arrow)
    {
        _currentArrow = arrow;
    }

    public void TryToReleaseArrow(SelectExitEventArgs args)
    {
        if (_currentArrow)
        {
            ForceDeselect(_currentArrow);
            ReleaseArrow();
        }
    }
    private void ForceDeselect(Arrow_M arrow)
    {
        _currentArrow.interactionManager.SelectExit((IXRSelectInteractor)this, arrow);
        //interactionManager.SelectExit((IXRSelectInteractor)this, _currentArrow);
       // _currentArrow.interactionManager.SelectExit(_currentArrow.firstInteractorSelecting, (IXRSelectInteractable)_currentArrow);
    }

    private void ReleaseArrow()
    {
        // Change layer to be ignored by notch
        _currentArrow.ChangeLayer();
        _currentArrow.Release(_pullInteraction.PullAmount);
        _prevArrow = _currentArrow;
        _currentArrow = null;
    }

    public override XRBaseInteractable.MovementType? selectedInteractableMovementTypeOverride
    {
        get { return XRBaseInteractable.MovementType.Instantaneous; }
    }

    void NotchSounds(AudioClip clip, float minPitch, float maxPitch, int id)
    {
        //SFXPlayer.Instance.PlaySFX(clip, transform.position, new SFXPlayer.PlayParameters()
        //{
        //    Pitch = Random.Range(minPitch, maxPitch),
        //    Volume = 1.0f,
        //    SourceID = id
        //});

        SoundManager.Instance?.PlaySound(transform.position, clip, 1, Random.Range(minPitch, maxPitch));
    }
}
