/// Tutorial from Unity: Creating bow and arrow gameplay in VR!

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class HapticManager_M 
{ 
    public static void Impulse(float amplitude, float duration, InputDevice inputDevice)
    {
        HapticCapabilities capabilities;
        if (inputDevice.TryGetHapticCapabilities(out capabilities))
        {
            if (capabilities.supportsImpulse)
            {
                uint channel = 0;
                inputDevice.SendHapticImpulse(channel, amplitude, duration);
            }
        }
    }
}
