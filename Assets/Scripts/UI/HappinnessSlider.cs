using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HappinnessSlider : MonoBehaviour
{
    private Slider slider;
    [SerializeField] private Image sliderFill;
    [SerializeField] private Color32[] sliderColours;

    private void Start()
    {
        slider = GetComponent<Slider>();
        slider.onValueChanged.AddListener(ChangeColour);
    }
    private void Update()
    {
        ChangeColour(slider.value);
    }

    private void ChangeColour(float value)
    {
        switch(value)
        {
            case < 20:
                sliderFill.color = sliderColours[0]; 
                break;
            case < 40:
                sliderFill.color = sliderColours[1];
                break;
            case < 60:
                sliderFill.color = sliderColours[2];
                break;
            case < 80:
                sliderFill.color = sliderColours[3];
                break;
            default:
                sliderFill.color = sliderColours[4];
                break;

        }
    }
}
