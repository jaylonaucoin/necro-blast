using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashlightToggle : MonoBehaviour
{
    public Light flashlight; // Reference to the flashlight

    void Start()
    {
        // Ensure the flashlight is on by default
        if (flashlight != null)
        {
            flashlight.enabled = true;
        }
    }

    void Update()
    {
      
        // Toggle flashlight with the "F" key
        if (Input.GetKeyDown(KeyCode.F) && flashlight != null)
        {
            flashlight.enabled = !flashlight.enabled;
        }
    }
}