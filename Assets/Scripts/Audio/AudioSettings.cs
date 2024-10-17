
using UnityEngine;
using UnityEngine.UI;

public class AudioSettings : MonoBehaviour
{
    [SerializeField] private AudioManager audioManager;
    public Slider crowdVolumeSlider;
    public Slider musicVolumeSlider;

    private void Start()
    {
        crowdVolumeSlider.value = 100;
        musicVolumeSlider.value = 100;
        
        crowdVolumeSlider.onValueChanged.AddListener((v) =>
        {
            audioManager.ambientSource.volume = 0.8f * v;
        });
        musicVolumeSlider.onValueChanged.AddListener((x) =>
        {
            audioManager.musicSource.volume = 0.8f * x;
        });
    }
    
}
