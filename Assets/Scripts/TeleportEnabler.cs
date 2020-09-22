using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class TeleportEnabler : MonoBehaviour
{
    public GameObject XRRig;
    public XRController leftController;
    public XRController rightController;

    private TeleportationProvider teleportationProvider;
    private XRGrabInteractable grabInteractable = null;

    // Start is called before the first frame update
    void Start()
    {
        teleportationProvider = XRRig.GetComponent<TeleportationProvider>();
        grabInteractable = GetComponent<XRGrabInteractable>();
        grabInteractable.onSelectEnter.AddListener(EnableTeleportation);
        grabInteractable.onSelectExit.AddListener(DisableTeleportation);
    }

    private void EnableTeleportation(XRBaseInteractor interactor)
    {
        teleportationProvider.gameObject.SetActive(true);
        teleportationProvider.enabled = true;
        // Set interactor rays to bezier curves.
        leftController.gameObject.GetComponent<XRRayInteractor>().lineType = XRRayInteractor.LineType.BezierCurve;
        rightController.gameObject.GetComponent<XRRayInteractor>().lineType = XRRayInteractor.LineType.BezierCurve;
    }

    private void DisableTeleportation(XRBaseInteractor interactor)
    {
        teleportationProvider.gameObject.SetActive(false);
        teleportationProvider.enabled = false;
        // Reset interactor rays to straight lines.
        leftController.gameObject.GetComponent<XRRayInteractor>().lineType = XRRayInteractor.LineType.StraightLine;
        rightController.gameObject.GetComponent<XRRayInteractor>().lineType = XRRayInteractor.LineType.StraightLine;
    }

    private void OnDestroy()
    {
        grabInteractable.onSelectEnter.RemoveAllListeners();
        grabInteractable.onSelectExit.RemoveAllListeners();
    }
}
