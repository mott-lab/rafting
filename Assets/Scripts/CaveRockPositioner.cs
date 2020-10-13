using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class CaveRockPositioner : MonoBehaviour
{
    public GameObject leftDepthMarker;
    public GameObject rightDepthMarker;
    //public bool isSelected = false;
    public XRGrabInteractable movingWand;
    private XRGrabInteractable grabInteractable;
    bool selectedByLeft = false;
    bool selectedByRight = false;

    // Start is called before the first frame update
    void Start()
    {
        grabInteractable = gameObject.GetComponent<XRGrabInteractable>();
        grabInteractable.onSelectEnter.AddListener(SelectRoutine);
        grabInteractable.onSelectExit.AddListener(DeselectRoutine);
    }

    private void SelectRoutine(XRBaseInteractor interactor)
    {
        if (interactor.gameObject.name.Equals("LeftHand Controller") && movingWand.isSelected)
        {
            gameObject.transform.position = leftDepthMarker.transform.position;
            selectedByLeft = true;
            selectedByRight = false;
        } else if (interactor.gameObject.name.Equals("RightHand Controller") && movingWand.isSelected)
        {
            gameObject.transform.position = rightDepthMarker.transform.position;
            selectedByRight = true;
            selectedByLeft = false;
        }
        Debug.Log(selectedByLeft);
        Debug.Log(selectedByRight);
    }

    private void DeselectRoutine(XRBaseInteractor interactor)
    {
        //isSelected = false;
        selectedByLeft = false;
        selectedByRight = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (grabInteractable.isSelected && movingWand.isSelected && selectedByRight)
        {
            grabInteractable.gameObject.transform.position = rightDepthMarker.transform.position;
        } else if (grabInteractable.isSelected && movingWand.isSelected && selectedByLeft)
        {
            grabInteractable.gameObject.transform.position = leftDepthMarker.transform.position;
        }
    }

    private void OnDestroy()
    {
        grabInteractable.onSelectEnter.RemoveListener(SelectRoutine);
        grabInteractable.onSelectExit.RemoveListener(DeselectRoutine);
    }
}
