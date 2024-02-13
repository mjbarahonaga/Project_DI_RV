/// Tutorial from Unity: Creating bow and arrow gameplay in VR!

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class HandInteractor_M : XRDirectInteractor
{

    [Header("Sounds")]
    public AudioClip BowGrabClip;
    public AudioClip ArrowGrabClip;

    public void ForceDeinteract()
    {
        if(firstInteractableSelected != null) 
            interactionManager.SelectExit(this, firstInteractableSelected);
    }

    public void ForceInteract(PullInteraction_M pullInteraction)
    {
        interactionManager.SelectEnter((IXRSelectInteractor)this, pullInteraction);
    }

    public void HandDetection(SelectEnterEventArgs args)
    {
        if(args.interactableObject is Arrow_M arrow)
        {
            arrow.SphereCollider.enabled = false;
            HandSounds(ArrowGrabClip, 3.5f, 3, .8f, 7);
        }

        if (args.interactableObject is Bow_M bow)
        {
            HandSounds(BowGrabClip, 2.5f, 2.5f, .8f, -3);
        }
    }

    void HandSounds(AudioClip clip, float minPitch, float maxPitch, float volume, int id)
    {
        //SFXPlayer.Instance.PlaySFX(clip, transform.position, new SFXPlayer.PlayParameters()
        //{
        //    Pitch = Random.Range(minPitch, maxPitch),
        //    Volume = volume,
        //    SourceID = id
        //});
    }
}
