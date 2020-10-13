using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ScaledWalkingEnabler : MonoBehaviour
{
    public GameObject XRRig;

    private ScaledWalkingProvider scaledWalkingProvider;
    private XRGrabInteractable grabInteractable = null;

    private WalkingProvider walkingProvider;

    private MeshCollider meshCollider;

    public XRController leftController;
    public XRController rightController;
    private XRInteractorLineVisual leftLineVisual;
    private XRInteractorLineVisual rightLineVisual;
    public GameObject leftDepthMarker;
    public GameObject rightDepthMarker;

    // Start is called before the first frame update
    void Start()
    {
        scaledWalkingProvider = XRRig.GetComponent<ScaledWalkingProvider>();
        walkingProvider = XRRig.GetComponent<WalkingProvider>();
        grabInteractable = GetComponent<XRGrabInteractable>();
        grabInteractable.onSelectEnter.AddListener(EnableScaledWalking);
        grabInteractable.onSelectExit.AddListener(DisableScaledWalking);
        meshCollider = GetComponent<MeshCollider>();

        leftLineVisual = leftController.GetComponent<XRInteractorLineVisual>();
        rightLineVisual = rightController.GetComponent<XRInteractorLineVisual>();
    }

    private void EnableScaledWalking(XRBaseInteractor interactor)
    {
        //meshCollider.enabled = false;
        scaledWalkingProvider.enabled = true;
        walkingProvider.enabled = false;
        if (interactor.gameObject.name.Equals("LeftHand Controller"))
        {
            leftLineVisual.enabled = false;
            leftDepthMarker.gameObject.SetActive(false);
        }
        else
        {
            rightLineVisual.enabled = false;
            rightDepthMarker.gameObject.SetActive(false);
        }
    }

    private void DisableScaledWalking(XRBaseInteractor interactor)
    {
        //meshCollider.enabled = true;
        scaledWalkingProvider.enabled = false;
        walkingProvider.enabled = true;
        if (interactor.gameObject.name.Equals("LeftHand Controller"))
        {
            leftLineVisual.enabled = true;
            leftDepthMarker.gameObject.SetActive(true);
        }
        else
        {
            rightLineVisual.enabled = true;
            rightDepthMarker.gameObject.SetActive(true);
        }
    }

    private void OnDestroy()
    {
        grabInteractable.onSelectEnter.RemoveListener(EnableScaledWalking);
        grabInteractable.onSelectExit.RemoveListener(DisableScaledWalking);
    }
}
