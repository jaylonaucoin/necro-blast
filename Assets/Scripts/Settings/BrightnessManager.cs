using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BrightnessManager : MonoBehaviour
{
    [SerializeField] private Slider brightnessSlider;
    [SerializeField] private Image brightnessOverlay;

    // Minimum brightness value to avoid a completely dark screen
    private const float minBrightness = 0.2f; // Adjust this value as needed
    private const float maxBrightness = 1.0f; // Full brightness

    private void Start()
    {
        // Load saved brightness value or set it to full brightness as the default
        float savedBrightness = PlayerPrefs.GetFloat("Brightness", maxBrightness);
        brightnessSlider.value = Mathf.Clamp(savedBrightness, minBrightness, maxBrightness);
        SetBrightness(brightnessSlider.value);

        // Add listener to update brightness when the slider value changes
        brightnessSlider.onValueChanged.AddListener(SetBrightness);
    }

    public void SetBrightness(float value)
    {
        // Ensure the value doesn't go below the minimum
        value = Mathf.Clamp(value, minBrightness, maxBrightness);

        if (brightnessOverlay != null)
        {
            // Adjust overlay alpha based on brightness value (inverse relationship)
            brightnessOverlay.color = new Color(0, 0, 0, 1 - value);
        }

        // Save the new brightness value
        PlayerPrefs.SetFloat("Brightness", value);
        PlayerPrefs.Save();
    }

    private void OnDestroy()
    {
        // Unsubscribe the listener to avoid memory leaks
        brightnessSlider.onValueChanged.RemoveListener(SetBrightness);
    }
}