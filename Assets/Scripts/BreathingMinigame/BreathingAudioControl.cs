using System.Collections;
using System.Collections.Generic;
using BreathingMinigame;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;

public class AudioController : MonoBehaviour
{
    private AudioSource audioSource;

    [Header("Audio Clips")]
    public AudioClip defaultClip;
    public AudioClip perfectClip;


    public CircleController circleController;

    private bool isPlaying = false;

    // Start is called before the first frame update
    void Start()
    {
       audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        //starts the audio when any key is pressed
        if (Input.anyKeyDown && !isPlaying)
        {
            isPlaying = true;
            audioSource.Play();
        }
        
    }

    //plays the perfect sound when the circle is in the perfect range
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("PerfectRange"))
        {
            audioSource.Stop();
            audioSource.clip = perfectClip;
            audioSource.Play();
        }
    }

    //plays the default sound when the circle is out of the perfect range
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("PerfectRange"))
        {
            audioSource.Stop();
            audioSource.clip = defaultClip;
            audioSource.Play();
        }
    }


}
