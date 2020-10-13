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

    private MeshCollider meshCollider;

    private GameObject[] teleportDestinations;

    private XRInteractorLineVisual leftLineVisual;
    private XRInteractorLineVisual rightLineVisual;
    public GameObject leftDepthMarker;
    public GameObject rightDepthMarker;

    // Start is called before the first frame update
    void Start()
    {
        teleportationProvider = XRRig.GetComponent<TeleportationProvider>();
        grabInteractable = GetComponent<XRGrabInteractable>();
        grabInteractable.onSelectEnter.AddListener(EnableTeleportation);
        grabInteractable.onSelectExit.AddListener(DisableTeleportation);

        meshCollider = GetComponent<MeshCollider>();

        teleportDestinations = GameObject.FindGameObjectsWithTag("TeleportDestination");

        foreach (GameObject dest in teleportDestinations)
        {
            dest.gameObject.SetActive(false);
        }

        leftLineVisual = leftController.GetComponent<XRInteractorLineVisual>();
        rightLineVisual = rightController.GetComponent<XRInteractorLineVisual>();
    }

    private void EnableTeleportation(XRBaseInteractor interactor)
    {
        // Disable mesh collider to prevent movement effects.
        //meshCollider.enabled = false;
        teleportationProvider.enabled = true;
        // Set interactor rays to bezier curves.
        //leftController.gameObject.GetComponent<XRRayInteractor>().lineType = XRRayInteractor.LineType.BezierCurve;
        //rightController.gameObject.GetComponent<XRRayInteractor>().lineType = XRRayInteractor.LineType.BezierCurve;

        foreach (GameObject dest in teleportDestinations)
        {
            dest.gameObject.SetActive(true);
        }
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

    private void DisableTeleportation(XRBaseInteractor interactor)
    {
        //meshCollider.enabled = true;
        teleportationProvider.enabled = false;
        // Reset interactor rays to straight lines.
        //leftController.gameObject.GetComponent<XRRayInteractor>().lineType = XRRayInteractor.LineType.StraightLine;
        //rightController.gameObject.GetComponent<XRRayInteractor>().lineType = XRRayInteractor.LineType.StraightLine;

        foreach (GameObject dest in teleportDestinations)
        {
            dest.gameObject.SetActive(false);
        }
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
        grabInteractable.onSelectEnter.RemoveListener(EnableTeleportation);
        grabInteractable.onSelectExit.RemoveListener(DisableTeleportation);
    }
}
