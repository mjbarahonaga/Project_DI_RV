using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ArrowSpawner_M : XRSimpleInteractable
{
    [SerializeField] private GameObject _arrowPrefab = default;
    
    [Header("Sound")]
    public AudioClip GrabClip;

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);
        if (args.interactorObject is XRBaseControllerInteractor controllerInteractor && controllerInteractor != null)
        {
            if(controllerInteractor.TryGetComponent(out HandInteractor_M hand))
                hand.interactionManager.SelectEnter((IXRSelectInteractor)hand, CreateArrow());
        }
    }

    private Arrow_M CreateArrow()
    {
        return Instantiate(_arrowPrefab).GetComponent<Arrow_M>();
    }
}
