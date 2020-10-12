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

    // Start is called before the first frame update
    void Start()
    {
        scaledWalkingProvider = XRRig.GetComponent<ScaledWalkingProvider>();
        walkingProvider = XRRig.GetComponent<WalkingProvider>();
        grabInteractable = GetComponent<XRGrabInteractable>();
        grabInteractable.onSelectEnter.AddListener(EnableScaledWalking);
        grabInteractable.onSelectExit.AddListener(DisableScaledWalking);
        meshCollider = GetComponent<MeshCollider>();
    }

    private void EnableScaledWalking(XRBaseInteractor interactor)
    {
        //meshCollider.enabled = false;
        scaledWalkingProvider.enabled = true;
        walkingProvider.enabled = false;
    }

    private void DisableScaledWalking(XRBaseInteractor interactor)
    {
        //meshCollider.enabled = true;
        scaledWalkingProvider.enabled = false;
        walkingProvider.enabled = true;
    }

    private void OnDestroy()
    {
        grabInteractable.onSelectEnter.RemoveListener(EnableScaledWalking);
        grabInteractable.onSelectExit.RemoveListener(DisableScaledWalking);
    }
}
