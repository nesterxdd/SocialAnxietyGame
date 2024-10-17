
using System;
using Controls;
using UnityEngine;
using Random = UnityEngine.Random;

public class AudioManager : MonoBehaviour
{
    private PlayerPanicControl _playerPanicControl;
    [Header("Audio Source")]
    public AudioSource ambientSource;
    public AudioSource musicSource;
    public AudioSource sfxSource;
    [Header("Audio Clips")] 
    public AudioClip crowdNoise;
    public AudioClip[] headphoneSongs;
    public AudioClip[] conversation;
    public AudioClip finishedMain;

    private void Start()
    {
        _playerPanicControl = PlayerPanicControl.instance;
        ambientSource.clip = crowdNoise;
        ambientSource.Play();
    }

    private void Update()
    {
        if (!Input.GetKeyDown(KeyCode.H)) return;
        if(_playerPanicControl.earphonesIn)
            StopMusic();
        else
        {
            PlayMusic();
        }
    }

    public void PlayMusic()
    {
        musicSource.clip = headphoneSongs[(Random.Range(0,headphoneSongs.Length))];
        musicSource.Play();
        _playerPanicControl.earphonesIn = true;
        ambientSource.Stop();
    }

    public void StopMusic()
    {
        musicSource.Stop();
        _playerPanicControl.earphonesIn = false;
        ambientSource.Play();
    }
}
