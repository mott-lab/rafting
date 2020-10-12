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
    // The Ray Offset Provider works with the XR Ray Interactor to provide a dynamic offset for raycasting.
    [AddComponentMenu("XRST/Interaction/RayOffsetProvider")]
    public class RayOffsetProvider : MonoBehaviour
    {
        // The XR Ray Interactor to provide a dynamic offset for.
        [SerializeField]
        [Tooltip("The XR Ray Interactor to provide a dynamic offset for.")]
        XRRayInteractor m_RayInteractor;
        public XRRayInteractor RayInteractor { get { return m_RayInteractor; } set { m_RayInteractor = value; } }

        // Whether the listeners have been added.
        bool listenersAdded;

        // Listener called by the interactor's OnSelectEnter interactor event.
        public void SetOffset(XRBaseInteractable interactable)
        {
            // If a local ray interactor exists.
            if (RayInteractor != null)
            {
                // Treat the base interactable as a grab interactable.
                XRGrabInteractable grabInteractable = interactable as XRGrabInteractable;
                // If it is a grab interactable.
                if (grabInteractable != null)
                {
                    // Set the grab interactable's attach transform to the ray interactor's transform (i.e., the offset).
                    grabInteractable.attachTransform = RayInteractor.transform;
                }
            }
        }

        // Listener called by the interactor's OnSelectExit interactor event.
        public void ResetOffset(XRBaseInteractable interactable)
        {
            // If a local ray interactor exists.
            if (RayInteractor != null)
            {
                // Treat the base interactable as a grab interactable.
                XRGrabInteractable grabInteractable = interactable as XRGrabInteractable;
                // If it is a grab interactable.
                if (grabInteractable != null)
                {
                    // Set the grab interactable's attach transform to null.
                    grabInteractable.attachTransform = null;
                }
            }
        }

        // Reset function for initializing the ray offset provider.
        void Reset()
        {
            // Attempt to fetch a local ray interactor.
            m_RayInteractor = GetComponent<XRRayInteractor>();

            // Did not find a local ray interactor.
            if (m_RayInteractor == null)
            {
                // Warn the developer.
                Debug.LogWarning("[" + gameObject.name + "][RayOffsetProvider]: Did not find a local XRRayInteractor attached to the same game object.");

                // Attepmt to fetch any ray interactor.
                m_RayInteractor = FindObjectOfType<XRRayInteractor>();

                // Did not find one.
                if (m_RayInteractor == null)
                {
                    Debug.LogWarning("[" + gameObject.name + "][RayOffsetProvider]: Did not find an XRRayInteractor in the scene.");
                }
                // Found one.
                else
                {
                    Debug.LogWarning("[" + gameObject.name + "][RayOffsetProvider]: Found an XRRayInteractor attached to " + m_RayInteractor.gameObject.name + ".");
                }
            }
        }

        // This function is called when the behaviour becomes disabled.
        void OnDisable()
        {
            // Attempt to remove the ResetOffset and SetOffset listeners from the events of the ray interactor.
            if (m_RayInteractor != null && listenersAdded)
            {
                // Remove the SetOffset function as a listener.
                m_RayInteractor.onSelectEnter.RemoveListener(SetOffset);
                // Remove the ResetOffset function as a listener.
                m_RayInteractor.onSelectExit.RemoveListener(ResetOffset);
                // Keep track of removing the listeners.
                listenersAdded = false;
            }
        }

        // This function is called when the object becomes enabled and active.
        void OnEnable()
        {
            // Attempt to add the ResetOffset and SetOffset listeners to the events of the ray interactor.
            if (m_RayInteractor != null && !listenersAdded)
            {
                // Add the SetOffset function as a listener.
                m_RayInteractor.onSelectEnter.AddListener(SetOffset);
                // Add the ResetOffset function as a listener.
                m_RayInteractor.onSelectExit.AddListener(ResetOffset);
                // Keep track of adding the listeners.
                listenersAdded = true;
            }
        }
    }
}
