using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GraphicsQualityManager : MonoBehaviour
{
    [SerializeField] private Slider graphicsQualitySlider;

    private void Start()
    {
        // Load saved quality level or set default (e.g., medium)
        int savedQuality = PlayerPrefs.GetInt("GraphicsQuality", 2);
        graphicsQualitySlider.value = savedQuality;
        SetGraphicsQuality((int)graphicsQualitySlider.value);

        // Add listener to update quality when the slider value changes
        graphicsQualitySlider.onValueChanged.AddListener(delegate { SetGraphicsQuality((int)graphicsQualitySlider.value); });
    }

    public void SetGraphicsQuality(int level)
    {
        QualitySettings.SetQualityLevel(level);
        PlayerPrefs.SetInt("GraphicsQuality", level);
        PlayerPrefs.Save();
    }
}