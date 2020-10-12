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
using cakeslice;

namespace UnityEngine.XR.Interaction.Toolkit
{
    // The OutlineProvider works with an interactor to provide an outline for hovered and selected interactables.
    [AddComponentMenu("XRST/Interaction/OutlineProvider")]
    public class OutlineProvider : MonoBehaviour
    {
        public bool movingWandPickedUp = false;

        // The interactor to provide outlines for.
        [SerializeField]
        [Tooltip("The interactor to provide outlines for.")]
        XRBaseInteractor m_Interactor;
        public XRBaseInteractor Interactor { get { return m_Interactor; } set { m_Interactor = value; } }

        // The OutlineEffect to use for outlines.
        [SerializeField]
        [Tooltip("The OutlineEffect to use for outlines.")]
        OutlineEffect m_OutlineEffect;
        public OutlineEffect OutlineEffect { get { return m_OutlineEffect; } set { m_OutlineEffect = value; } }

        // Whether the listeners have been added.
        bool listenersAdded;

        // Listener called by the interactor's OnHoverEnter interactor event.
        public void OnHoverEnterOutline(XRBaseInteractable interactable)
        {
            //Debug.Log(interactable.gameObject.layer);
            // If an outline effect exists.
            if (OutlineEffect != null)
            {
                // Attempt to get an outline component on the interactable.
                Outline outline = interactable.GetComponent<Outline>();

                // Create one if there isn't one already.
                if (outline == null)
                {
                    outline = interactable.gameObject.AddComponent<Outline>();
                }
                //Debug.Log(interactable.gameObject.layer);
                // If object is in noninteractable layer, set color to red
                       
                //outline.color = 1;
                outline.color = 1;
                if (interactable.gameObject.tag.Equals("BigRock") && !movingWandPickedUp)
                {

                    m_OutlineEffect.lineColor1.r = 255;
                    m_OutlineEffect.lineColor1.g = 0;
                    m_OutlineEffect.lineColor1.b = 0;
                    m_OutlineEffect.fillAmount = 0.000001f;
                } else
                {
                    m_OutlineEffect.lineColor1.r = 255;
                    m_OutlineEffect.lineColor1.g = 255;
                    m_OutlineEffect.lineColor1.b = 0;
                    m_OutlineEffect.fillAmount = 0.000001f;
                }
                
            }
        }

        // Listener called by the interactor's OnHoverExit interactor event.
        public void OnHoverExitOutline(XRBaseInteractable interactable)
        {
            // If an outline effect exists.
            if (OutlineEffect != null)
            {
                // Attempt to get an outline component on the interactable.
                Outline outline = interactable.GetComponent<Outline>();

                // Create one if there isn't one already.
                if (outline == null)
                {
                    outline = interactable.gameObject.AddComponent<Outline>();
                }

                // Set the outline's color to the line color 0 effect.
                outline.color = 0;
            }
        }

        // Listener called by the interactor's OnSelectEnter interactor event.
        public void OnSelectEnterOutline(XRBaseInteractable interactable)
        {
            // If an outline effect exists.
            if (OutlineEffect != null)
            {
                // Attempt to get an outline component on the interactable.
                Outline outline = interactable.GetComponent<Outline>();

                // Create one if there isn't one already.
                if (outline == null)
                {
                    outline = interactable.gameObject.AddComponent<Outline>();
                }

                // Set the outline's color to the line color 2 effect.
                outline.color = 2;
            }
        }

        // Listener called by the interactor's OnSelectExit interactor event.
        public void OnSelectExitOutline(XRBaseInteractable interactable)
        {
            // If an outline effect exists.
            if (OutlineEffect != null)
            {
                // Attempt to get an outline component on the interactable.
                Outline outline = interactable.GetComponent<Outline>();

                // Create one if there isn't one already.
                if (outline == null)
                {
                    outline = interactable.gameObject.AddComponent<Outline>();
                }

                // Set the outline's color to the line color 0 effect.
                outline.color = 0;
            }
        }

        // Reset function for initializing the OutlineProvider.
        void Reset()
        {
            // Attempt to fetch a local interactor.
            m_Interactor = GetComponent<XRBaseInteractor>();

            // Did not find a interactor.
            if (m_Interactor == null)
            {
                // Warn the developer.
                Debug.LogWarning("[" + gameObject.name + "][OutlineProvider]: Did not find a local interactor attached to the same game object.");

                // Attepmt to fetch any interactor.
                m_Interactor = FindObjectOfType<XRBaseInteractor>();

                // Did not find one.
                if (m_Interactor == null)
                {
                    Debug.LogWarning("[" + gameObject.name + "][OutlineProvider]: Did not find an interactor in the scene.");
                }
                // Found one.
                else
                {
                    Debug.LogWarning("[" + gameObject.name + "][OutlineProvider]: Found an interactor attached to " + m_Interactor.gameObject.name + ".");
                }
            }

            // Attempt to fetch a local OutlineEffect.
            m_OutlineEffect = GetComponent<OutlineEffect>();

            // Did not find an OutlineEffect.
            if (m_OutlineEffect == null)
            {
                // Warn the developer.
                Debug.LogWarning("[" + gameObject.name + "][OutlineProvider]: Did not find a local OutlineEffect attached to the same game object.");

                // Attepmt to fetch any OutlineEffect.
                m_OutlineEffect = FindObjectOfType<OutlineEffect>();

                // Did not find one.
                if (m_OutlineEffect == null)
                {
                    Debug.LogWarning("[" + gameObject.name + "][OutlineProvider]: Did not find an OutlineEffect in the scene.");
                }
                // Found one.
                else
                {
                    Debug.LogWarning("[" + gameObject.name + "][OutlineProvider]: Found an OutlineEffect attached to " + m_OutlineEffect.gameObject.name + ".");
                }
            }
        }

        // This function is called when the behaviour becomes disabled.
        void OnDisable()
        {
            // Attempt to remove the listeners from the events of the interactor.
            if (m_Interactor != null && listenersAdded)
            {
                // Remove the OnHoverEnterOutline function as a listener.
                m_Interactor.onHoverEnter.RemoveListener(OnHoverEnterOutline);
                // Remove the OnHoverExitOutline function as a listener.
                m_Interactor.onHoverExit.RemoveListener(OnHoverExitOutline);
                // Remove the OnSelectEnterOutline function as a listener.
                m_Interactor.onSelectEnter.RemoveListener(OnSelectEnterOutline);
                // Remove the OnSelectExitOutline function as a listener.
                m_Interactor.onSelectExit.RemoveListener(OnSelectExitOutline);
                // Keep track of removing the listeners.
                listenersAdded = false;
            }
        }

        // This function is called when the object becomes enabled and active.
        void OnEnable()
        {
            // Attempt to add the listeners to the events of the interactor.
            if (m_Interactor != null && !listenersAdded)
            {
                // Add the OnHoverEnterOutline function as a listener.
                m_Interactor.onHoverEnter.AddListener(OnHoverEnterOutline);
                // Add the OnHoverExitOutline function as a listener.
                m_Interactor.onHoverExit.AddListener(OnHoverExitOutline);
                // Add the OnSelectEnterOutline function as a listener.
                m_Interactor.onSelectEnter.AddListener(OnSelectEnterOutline);
                // Add the OnSelectExitOutline function as a listener.
                m_Interactor.onSelectExit.AddListener(OnSelectExitOutline);
                // Keep track of adding the listeners.
                listenersAdded = true;
            }
        }
    }
}
