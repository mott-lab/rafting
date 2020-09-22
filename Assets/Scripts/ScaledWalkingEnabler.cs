using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ScaledWalkingEnabler : MonoBehaviour
{
    public GameObject XRRig;

    private ScaledWalkingProvider scaledWalkingProvider;
    private XRGrabInteractable grabInteractable = null;

    // Start is called before the first frame update
    void Start()
    {
        scaledWalkingProvider = XRRig.GetComponent<ScaledWalkingProvider>();
        grabInteractable = GetComponent<XRGrabInteractable>();
        grabInteractable.onSelectEnter.AddListener(EnableScaledWalking);
        grabInteractable.onSelectExit.AddListener(DisableScaledWalking);
    }

    private void EnableScaledWalking(XRBaseInteractor interactor)
    {
        scaledWalkingProvider.gameObject.SetActive(true);
        scaledWalkingProvider.enabled = true;
    }

    private void DisableScaledWalking(XRBaseInteractor interactor)
    {
        scaledWalkingProvider.gameObject.SetActive(false);
        scaledWalkingProvider.enabled = false;
    }

    private void OnDestroy()
    {
        grabInteractable.onSelectEnter.RemoveAllListeners();
        grabInteractable.onSelectExit.RemoveAllListeners();
    }
}
