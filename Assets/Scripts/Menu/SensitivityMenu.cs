using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class SensitivityMenu : OptionsMenuTab
{
    [SerializeField] private Slider sensitivitySlider;
    [SerializeField] private TMP_InputField sensitivityText;
    [SerializeField] private float maxSensitivity = 10;
    

    private float sensitivity;

    public float Sensitivity
    {
        get
        {
            return sensitivity;
        }
        set
        {
            float sliderValue = value / maxSensitivity;
            sensitivitySlider.value = sliderValue;
        }
    }

    private void Update()
    {
        sensitivity = sensitivitySlider.value * maxSensitivity;
        sensitivity = Mathf.Round(sensitivity * 100) / 100;
        if (!sensitivityText.isFocused) {
            sensitivityText.text = sensitivity.ToString("0.00");
        }
    }

    public void OnEndEdit_SensitivityText() {
        try
        {
            float sensitivity = float.Parse(sensitivityText.text);
            sensitivitySlider.value = sensitivity / maxSensitivity;
        }
        catch
        { return; }
       
    }
}
