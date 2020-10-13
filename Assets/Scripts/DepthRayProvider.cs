/*
*   Copyright (C) 2020 University of Central Florida, created by Dr. Ryan P. McMahan.
*
*   This program is free software: you can redistribute it and/or modify
*   it under the terms of the GNU General Public License as published by
*   the Free Software Foundation, either version 3 of the License, or
*   (at your option) any later version.
*
*   This program is distributed in the hope that it will be useful,
*   but WITHOUT ANY WARRANTY; without even the implied warranty of
*   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
*   GNU General Public License for more details.
*
*   You should have received a copy of the GNU General Public License
*   along with this program.  If not, see <http://www.gnu.org/licenses/>.
*
*   Primary Author Contact:  Dr. Ryan P. McMahan <rpm@ucf.edu>
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace UnityEngine.XR.Interaction.Toolkit
{
    // The DepthRayProvider works with an XRRayInteractor to provide a depth ray technique.
    [AddComponentMenu("XRST/Interaction/DepthRayProvider")]
    public class DepthRayProvider : MonoBehaviour
    {
        // The XRRayInteractor to change into a depth ray technique.
        [SerializeField]
        [Tooltip("The XRRayInteractor to change into a depth ray technique.")]
        XRRayInteractor m_Interactor;
        public XRRayInteractor Interactor { get { return m_Interactor; } set { m_Interactor = value; } }

        // The XRController to use for moving the depth marker forward and backward.
        [SerializeField]
        [Tooltip("The XRController to use for moving the depth marker forward and backward.")]
        XRController m_Controller;
        public XRController Controller { get { return m_Controller; } set { m_Controller = value; } }

        // The GameObject that represents the depth marker.
        [SerializeField]
        [Tooltip("The GameObject that represents the depth marker.")]
        GameObject m_DepthMarker;
        public GameObject DepthMarker { get { return m_DepthMarker; } set { m_DepthMarker = value; } }

        // The speed that the depth marker moves in and out.
        [SerializeField]
        [Tooltip("The speed that the depth marker moves in and out.")]
        float m_Speed = 1.0f;
        public float Speed { get { return m_Speed; } set { m_Speed = value; } }

        // List of valid targets.
        List<XRBaseInteractable> validTargets;

        // Reset function for initializing the DepthRayProvider.
        void Reset()
        {
            // Attempt to fetch a local interactor.
            m_Interactor = GetComponent<XRRayInteractor>();

            // Did not find a local interactor.
            if (m_Interactor == null)
            {
                // Warn the developer.
                Debug.LogWarning("[" + gameObject.name + "][DepthRayProvider]: Did not find a local XRRayInteractor attached to the same game object.");

                // Attepmt to fetch any interactor.
                m_Interactor = FindObjectOfType<XRRayInteractor>();

                // Did not find one.
                if (m_Interactor == null)
                {
                    Debug.LogWarning("[" + gameObject.name + "][DepthRayProvider]: Did not find an XRRayInteractor in the scene.");
                }
                // Found one.
                else
                {
                    Debug.LogWarning("[" + gameObject.name + "][DepthRayProvider]: Found an XRRayInteractor attached to " + m_Interactor.gameObject.name + ".");
                }
            }

            // Attempt to fetch a local controller.
            m_Controller = GetComponent<XRController>();

            // Did not find a local controller.
            if (m_Controller == null)
            {
                // Warn the developer.
                Debug.LogWarning("[" + gameObject.name + "][DepthRayProvider]: Did not find a local XRController attached to the same game object.");

                // Attepmt to fetch any controller.
                m_Controller = FindObjectOfType<XRController>();

                // Did not find one.
                if (m_Controller == null)
                {
                    Debug.LogWarning("[" + gameObject.name + "][DepthRayProvider]: Did not find an XRController in the scene.");
                }
                // Found one.
                else
                {
                    Debug.LogWarning("[" + gameObject.name + "][DepthRayProvider]: Found an XRController attached to " + m_Controller.gameObject.name + ".");
                }
            }
        }

        // Start is called before the first frame update.
        void Start()
        {
            // Disable any colliders on the depth marker.
            List<Collider> markerColliders = new List<Collider>(DepthMarker.GetComponents<Collider>());
            for (int i = 0; i < markerColliders.Count; i++)
            {
                markerColliders[i].enabled = false;
            }

            // Create an empty list of valid targets and layers.
            validTargets = new List<XRBaseInteractable>();
        }

        // Update is called once per frame.
        void Update()
        {
            // Determine whether the depth marker is being moved forward or backward.
            bool moving = false;
            float magnitude = 0.0f;

            // Get the primary 2D axis and button.
            InputFeatureUsage<Vector2> axisFeature = CommonUsages.primary2DAxis;
            InputFeatureUsage<bool> buttonFeature = CommonUsages.primary2DAxisClick;

            // If the controller is valid and enabled.
            if (Controller != null && Controller.enableInputActions)
            {
                // Fetch the controller's device.
                InputDevice device = Controller.inputDevice;

                // Try to get the current state of the device's primary 2D axis and button.
                Vector2 axis;
                bool button;
                if (device.TryGetFeatureValue(axisFeature, out axis) && device.TryGetFeatureValue(buttonFeature, out button))
                {
                    // Activate moving and add the magnitude if the button is down.
                    if (button)
                    {
                        moving = true;
                        magnitude += axis.y;
                    }
                }
            }

            // If moving is active, move the marker.
            if (moving)
            {
                // Calculate the movement of the marker based on the controller's direction.
                Vector3 movement = Controller.transform.forward;

                // Scale the movement by the magnitude and speed per second (which requires deltaTime).
                movement *= magnitude * Speed * Time.deltaTime;

                // Determine the marker's new world location.
                Vector3 newLocation = DepthMarker.transform.position + movement;

                // Check whether the marker is on the positive side of the ray-cast.
                Plane rayCastPlane = new Plane(Controller.transform.forward, Controller.transform.position);

                // Reset to the controller position, if not.
                if (!rayCastPlane.SameSide(Controller.transform.position + Controller.transform.forward, newLocation))
                {
                    newLocation = Controller.transform.position;
                }

                // Set that new location.
                DepthMarker.transform.position = newLocation;
            }

            // Re-enable any old targets.
            for (int i = 0; i < validTargets.Count; i++)
            {
                // Fetch the interactable's collider.
                Collider validCollider = validTargets[i].GetComponent<Collider>();

                // If it exists.
                if (validCollider != null)
                {
                    // Re-enable the interactable's collider, if necessary.
                    if (validCollider.enabled != true)
                    {
                        validCollider.enabled = true;
                    }

                    // Make the interactable's rigidbody not kinematic, if necessary.
                    if (validCollider.attachedRigidbody.isKinematic != false)
                    {
                        validCollider.attachedRigidbody.isKinematic = false;
                    }
                }
            }

            // If the interactor is valid.
            if (Interactor != null)
            {
                // Fetch the interactor's current list of valid interactables.
                Interactor.GetValidTargets(validTargets);

                // Determine the closest interactable.
                int closest = -1;
                float closestDistance = Mathf.Infinity;

                // For each interactable.
                for (int i = 0; i < validTargets.Count; i++)
                {
                    // Fetch the interactable's collider.
                    Collider interactableCollider = validTargets[i].GetComponent<Collider>();

                    // Ensure the collider is valid.
                    if (interactableCollider != null)
                    {
                        // Fetch the collider's bounds.
                        Bounds interactableBounds = interactableCollider.bounds;

                        // Determine its closest point to our depth marker.
                        Vector3 closestPoint = interactableBounds.ClosestPoint(DepthMarker.transform.position);

                        // Determine its distance to our collider.
                        float distance = Vector3.Distance(closestPoint, DepthMarker.transform.position);

                        // Keep track of the closest distance and interactable.
                        if (distance < closestDistance)
                        {
                            closest = i;
                            closestDistance = distance;
                        }
                    }
                }

                // If a closest interactable was found.
                if (closest > -1)
                {
                    // Disable all other interactables.
                    for (int i = 0; i < validTargets.Count; i++)
                    {
                        // If not the closest.
                        if (i != closest)
                        {
                            // Fetch the interactable's collider.
                            Collider interactableCollider = validTargets[i].GetComponent<Collider>();

                            // Ensure the collider is valid.
                            if (interactableCollider != null)
                            {
                                // Make the interactable's rigidbody kinematic.
                                interactableCollider.attachedRigidbody.isKinematic = true;

                                // Disable the interactable's collider.
                                interactableCollider.enabled = false;
                            }
                        }
                    }
                }
            }
        }
    }
}
