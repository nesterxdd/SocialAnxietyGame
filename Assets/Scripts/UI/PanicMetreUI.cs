using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PanicMetreUI : MonoBehaviour
{
    [Header("Panic Metre")]
    public Image panicIcon;
    public Slider panicSlider;
    public Slider confidenceSlider;

    private void Start()
    {
        panicSlider.value = 0;
        confidenceSlider.value = 0;
    }
}
