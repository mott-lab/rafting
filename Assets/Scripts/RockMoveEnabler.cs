using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class RockMoveEnabler : MonoBehaviour
{
    public GameObject XRRig;
    public GameObject leftController;
    public GameObject rightController;

    public GameObject bigRock;

    private XRGrabInteractable rockInteractable;
    private XRTintInteractableVisual rockInteractableTint;

    private RayOffsetProvider leftRayOffsetProvider;
    private RayOffsetProvider rightRayOffsetProvider;
    private XRGrabInteractable grabInteractable;

    private WalkingProvider walkingProvider;

    private OutlineProvider leftOutlineProvider;
    private OutlineProvider rightOutlineProvider;

    // Start is called before the first frame update
    void Start()
    {
        leftRayOffsetProvider = leftController.GetComponent<RayOffsetProvider>();
        rightRayOffsetProvider = rightController.GetComponent<RayOffsetProvider>();

        // Enable or disable rock movement depending on whether wand is picked up
        grabInteractable = GetComponent<XRGrabInteractable>();
        grabInteractable.onSelectEnter.AddListener(EnableRockMove);
        grabInteractable.onSelectExit.AddListener(DisableRockMove);

        rockInteractable = bigRock.gameObject.GetComponent<XRGrabInteractable>();
        //rockInteractable.enabled = false;
        //rockInteractableTint = bigRock.gameObject.GetComponent<XRTintInteractableVisual>();

        leftOutlineProvider = leftController.GetComponent<OutlineProvider>();
        rightOutlineProvider = rightController.GetComponent<OutlineProvider>();
    }

    private void EnableRockMove(XRBaseInteractor interactor)
    {
        leftRayOffsetProvider.enabled = true;
        rightRayOffsetProvider.enabled = true;

        bigRock.layer = 9;

        rockInteractable.trackPosition = true;
        rockInteractable.trackRotation = true;

        leftOutlineProvider.movingWandPickedUp = true;
        rightOutlineProvider.movingWandPickedUp = true;
        //rockInteractable.enabled = true;
    }

    private void DisableRockMove(XRBaseInteractor interactor)
    {
        leftRayOffsetProvider.enabled = false;
        rightRayOffsetProvider.enabled = false;
        //rockInteractable.enabled = false;
        bigRock.layer = 11;
        rockInteractable.trackPosition = false;
        rockInteractable.trackRotation = false;

        leftOutlineProvider.movingWandPickedUp = false;
        rightOutlineProvider.movingWandPickedUp = false;
    }

    private void OnDestroy()
    {
        grabInteractable.onSelectEnter.RemoveListener(EnableRockMove);
        grabInteractable.onSelectExit.RemoveListener(DisableRockMove);
    }
}
