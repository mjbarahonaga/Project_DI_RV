using System.Linq;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using Ludus.XRIT.Interactables;

public class GameplayHand : BaseHand
{
    // The interactor we react to
    [SerializeField] private XRBaseInteractor targetInteractor = null;
    private PoseContainer poseContainer;
    private Transform originalParent;

    protected override void Awake()
    {
	    base.Awake();
	    targetInteractor = GetComponentInParent<XRBaseInteractor>();
        originalParent = transform.parent;
    }

    private void OnEnable()
    {
        // Subscribe to selected events
        targetInteractor.selectEntered.AddListener(TryApplyObjectPose);
        targetInteractor.selectExited.AddListener(TryApplyDefaultPose);
    }

    private void OnDisable()
    {
	    ApplyDefaultPose();
        // Unsubscribe to selected events
        targetInteractor.selectEntered.RemoveListener(TryApplyObjectPose);
        targetInteractor.selectExited.RemoveListener(TryApplyDefaultPose);
    }

    private void TryApplyObjectPose(SelectEnterEventArgs args)
    {
        if (args.interactableObject is XRBaseInteractable interactable)
        {
            // Try and get pose container, and apply
            if (interactable.TryGetComponent(out PoseContainer poseContainer))
            {
                this.poseContainer = poseContainer;
                ApplyPose(poseContainer.pose);
            }
        }
    }

    private void TryApplyDefaultPose(SelectExitEventArgs args)
    {
        if (args.interactableObject is XRBaseInteractable interactable)
        {
            // Try and get pose container, and apply
            if (interactable.TryGetComponent(out PoseContainer poseContainer))
            {
                this.poseContainer = null;
                ApplyDefaultPose();
            }
        }
    }

    public override void ApplyOffset(Vector3 position, Quaternion rotation)
    {
        FixedHandGripBehaviour grip = null;
        if (poseContainer != null)
        { 
            grip = poseContainer.transform.GetComponent<FixedHandGripBehaviour>();
        }
        
        if (grip == null)
        {
            transform.SetParent(originalParent, true);
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            
            // Invert since the we're moving the attach point instead of the hand
            Vector3 finalPosition = position * -1.0f;
            Quaternion finalRotation = Quaternion.Inverse(rotation);

            // Since it's a local position, we can just rotate around zero
            finalPosition = finalPosition.RotatePointAroundPivot(Vector3.zero, finalRotation.eulerAngles);

            // Set the position and rotach of attach
            targetInteractor.attachTransform.localPosition = finalPosition;
            targetInteractor.attachTransform.localRotation = finalRotation;
        }
        else
        {
            transform.SetParent(poseContainer.transform, true);

            HandInfo handInfo = poseContainer.pose.GetHandInfo(handType);
            transform.localRotation = Quaternion.identity * handInfo.attachRotation;
            transform.localPosition = handInfo.attachPosition;
        }
    }

    private void OnValidate()
    {
        // Let's have this done automatically, but not hide the requirement
        if (!targetInteractor)
        {
            targetInteractor = GetComponentInParent<XRBaseInteractor>();
        }
    }
}