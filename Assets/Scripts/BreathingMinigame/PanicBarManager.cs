using System.Collections;
using System.Collections.Generic;
using BreathingMinigame;
using UnityEngine;
using UnityEngine.UI;

public class PanicLevel : MonoBehaviour
{
    // Start is called before the first frame update

    private Image panicBar;

    public CircleController CircleController;

    void Start()
    {
        panicBar = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        panicBar.fillAmount = CircleController.timerTime / CircleController.durationTime;
    }
}
