using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class HeadSteeringEnabler : MonoBehaviour
{
    public GameObject XRRig;

    private HeadSteeringProvider headSteeringProvider;
    private XRGrabInteractable grabInteractable = null;

    public XRController leftController;
    public XRController rightController;
    private XRInteractorLineVisual leftLineVisual;
    private XRInteractorLineVisual rightLineVisual;
    public GameObject leftDepthMarker;
    public GameObject rightDepthMarker;
    private DepthRayProvider leftDepthRayProvider;
    private DepthRayProvider rightDepthRayProvider;

    // Start is called before the first frame update
    void Start()
    {
        headSteeringProvider = XRRig.GetComponent<HeadSteeringProvider>();
        grabInteractable = GetComponent<XRGrabInteractable>();
        leftDepthRayProvider = leftController.GetComponent<DepthRayProvider>();
        rightDepthRayProvider = rightController.GetComponent<DepthRayProvider>();

        grabInteractable.onSelectEnter.AddListener(EnableHeadSteering);
        grabInteractable.onSelectExit.AddListener(DisableHeadSteering);

        leftLineVisual = leftController.GetComponent<XRInteractorLineVisual>();
        rightLineVisual = rightController.GetComponent<XRInteractorLineVisual>();
    }

    private void EnableHeadSteering(XRBaseInteractor interactor)
    {
        //meshCollider.enabled = false;
        headSteeringProvider.enabled = true;
        if (interactor.gameObject.name.Equals("LeftHand Controller"))
        {
            leftLineVisual.enabled = false;
            leftDepthMarker.gameObject.SetActive(false);
            leftDepthRayProvider.enabled = false;
        }
        else
        {
            rightLineVisual.enabled = false;
            rightDepthMarker.gameObject.SetActive(false);
            rightDepthRayProvider.enabled = false;
        }
    }

    private void DisableHeadSteering(XRBaseInteractor interactor)
    {
        //meshCollider.enabled = true;
        headSteeringProvider.enabled = false;
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
        grabInteractable.onSelectEnter.RemoveListener(EnableHeadSteering);
        grabInteractable.onSelectExit.RemoveListener(DisableHeadSteering);
    }
}
