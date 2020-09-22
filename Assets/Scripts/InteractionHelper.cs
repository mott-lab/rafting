using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace UnityEngine.XR.Interaction.Toolkit
{
    public class InteractionHelper : MonoBehaviour
    {

        public XRController leftHandController;
        public XRController rightHandController;
        

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            InputFeatureUsage<float> gripFeature = CommonUsages.grip;
            InputDevice leftDevice = leftHandController.inputDevice;

            // Try to get the current state of the device's grip.
            float grip;
            if (leftDevice.TryGetFeatureValue(gripFeature, out grip))
            {
                
            }
        }
    }
}