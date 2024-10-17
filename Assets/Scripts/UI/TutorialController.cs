
using System;
using TMPro;
using UnityEngine;
using UnityEngine.Video;

public class TutorialController : MonoBehaviour
{
    [Header("Tutorial Info")]
    [SerializeField] private TutorialSO[] tutorialObjects;
    private int tutorialIndex;
    [SerializeField] private TMP_Text header;
    [SerializeField] private TMP_Text contents;

    [Header("Tutorial Control")] 
    public VideoPlayer videoPlayer;
    [SerializeField] private GameObject endTutorial;
    [SerializeField] private GameObject[] progressButtons;
    [SerializeField] private Color32[] colours;
    public bool tutorialActive;

    private void Start()
    {
        StartTutorial();
    }

    private void SetTutorial()
    {
        if (tutorialIndex == 0)
        {
            progressButtons[0].SetActive(false);
            progressButtons[1].SetActive(true);
            endTutorial.SetActive(false);
        }
        else if (tutorialIndex == tutorialObjects.Length - 1)
        {
            progressButtons[1].SetActive(false);
            progressButtons[0].SetActive(true);
            endTutorial.SetActive(true);
        }
        else
        {
            progressButtons[0].SetActive(true);
            progressButtons[1].SetActive(true);
            endTutorial.SetActive(false);
        }
        
        var tutorial = tutorialObjects[tutorialIndex];
        videoPlayer.clip = tutorial.video;
        header.text = tutorial.header;
        contents.text = tutorial.contents;
        videoPlayer.frame = 0;
        videoPlayer.isLooping = true;
        videoPlayer.Play();
    }
    
    public void StartTutorial()
    {
        tutorialIndex = 0;
        SetTutorial();
    }
    
    public void TutorialPlus()
    {
        tutorialIndex++;
        SetTutorial();
    }

    public void TutorialMinus()
    {
        tutorialIndex--;
        SetTutorial();
    }

    public void EndTutorial()
    {
        tutorialActive = false;
    }
}
