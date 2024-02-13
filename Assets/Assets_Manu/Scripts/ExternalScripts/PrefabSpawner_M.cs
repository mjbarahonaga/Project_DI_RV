/// Tutorial from Unity: Creating bow and arrow gameplay in VR!

using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class PrefabSpawner_M : XRSocketInteractor
{
    [SerializeField] private GameObject _prefab = default;
    public Arrow_M CurrentArrow;

    [Header("Sound")]
    public AudioClip GrabClip;

    protected override void Awake()
    {
        base.Awake();
        CreateAndSelectPrefab();
        //SetAttachOffset(CurrentArrow);
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);
        CreateAndSelectPrefab();
    }

    private void CreateAndSelectPrefab()
    {
        Arrow_M arrow = CreatePrefab();
        SelectPrefab(arrow);
    }

    private Arrow_M CreatePrefab()
    {
        CurrentArrow = Instantiate(_prefab, attachTransform.position, transform.rotation, attachTransform).GetComponent<Arrow_M>();
        return CurrentArrow;
    }

    private void SelectPrefab(Arrow_M arrow)
    {

        //var args = new SelectEnterEventArgs
        //{
        //    interactableObject = arrow,
        //    interactorObject = this
        //};
        //arrow.OnSelectEntering(args);
        //arrow.OnSelectEntered(args);
        //this.OnSelectEntered(args);

        if (interactionManager != null && arrow != null)
        {
            interactionManager.SelectEnter((IXRSelectInteractor)this, arrow);
        }
        

        //this.interactionManager.SelectEnter((IXRSelectInteractor)this, arrow);
        //arrow.interactionManager.SelectEnter((IXRSelectInteractor)this, arrow);
    }

    //private void SetAttachOffset(XRGrabInteractable interactable)
    //{

    //   //AttachOffset = interactable.attachTransform.localPosition;
    //   interactable.attachTransform = attachTransform;
    //}

    public void ForceDeinteract(XRBaseInteractable interactable)
    {
        var args = new SelectExitEventArgs
        {
            interactableObject = interactable,
            interactorObject = this
        };
        OnSelectExited(args);
        //interactable.interactionManager.SelectExit((IXRSelectInteractor)this, interactable);
    }
}
