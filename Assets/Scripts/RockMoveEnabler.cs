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

    private XRInteractorLineVisual leftLineVisual;
    private XRInteractorLineVisual rightLineVisual;



    public GameObject leftDepthMarker;
    public GameObject rightDepthMarker;

    // Start is called before the first frame update
    void Start()
    {
        leftRayOffsetProvider = leftController.GetComponent<RayOffsetProvider>();
        rightRayOffsetProvider = rightController.GetComponent<RayOffsetProvider>();



        leftLineVisual = leftController.GetComponent<XRInteractorLineVisual>();
        rightLineVisual = rightController.GetComponent<XRInteractorLineVisual>();

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
        if (interactor.gameObject.name.Equals("LeftHand Controller"))
        {
            leftLineVisual.enabled = false;
            leftDepthMarker.gameObject.SetActive(false);
            rightRayOffsetProvider.enabled = true;
            //rightDepthMarker.gameObject.SetActive(true);
            rightOutlineProvider.movingWandPickedUp = true;
        }
        else
        {
            rightLineVisual.enabled = false;
            rightDepthMarker.gameObject.SetActive(false);
            leftRayOffsetProvider.enabled = true;
            //leftDepthMarker.gameObject.SetActive(true);
            leftOutlineProvider.movingWandPickedUp = true;
        }

        rockInteractable.trackPosition = true;
        rockInteractable.trackRotation = true;

        bigRock.layer = 9;
        //rockInteractable.enabled = true;
    }

    private void DisableRockMove(XRBaseInteractor interactor)
    {
        if (interactor.gameObject.name.Equals("LeftHand Controller"))
        {
            leftLineVisual.enabled = true;
            leftDepthMarker.gameObject.SetActive(true);
            rightRayOffsetProvider.enabled = false;
            rightDepthMarker.gameObject.SetActive(false);
            rightOutlineProvider.movingWandPickedUp = false;
        }
        else
        {
            rightLineVisual.enabled = true;
            rightDepthMarker.gameObject.SetActive(true);
            leftRayOffsetProvider.enabled = false;
            leftDepthMarker.gameObject.SetActive(false);
            leftOutlineProvider.movingWandPickedUp = false;
        }

        //rockInteractable.enabled = false;
        bigRock.layer = 11;
        rockInteractable.trackPosition = false;
        rockInteractable.trackRotation = false;
    }

    private void OnDestroy()
    {
        grabInteractable.onSelectEnter.RemoveListener(EnableRockMove);
        grabInteractable.onSelectExit.RemoveListener(DisableRockMove);
    }
}
