using System.Collections;
using UnityEngine;

public class AudioTrigger : MonoBehaviour
{
    private AudioSource audioSource;

    // Array of audio clips
    public AudioClip[] audioClips;

    // Corresponding array of volume levels for each audio clip
    public float[] volumeLevels;
    // Fade out duration
    public float fadeDuration = 2.0f;

    void Start()
    {
        // Get the AudioSource component attached to the same GameObject
        audioSource = GetComponent<AudioSource>();

        // Ensure audioClips and volumeLevels arrays are properly set
        if (audioClips.Length != volumeLevels.Length)
        {
            Debug.LogError("The length of audioClips and volumeLevels arrays must be the same.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the collider that entered is tagged as "Player"
        if (other.CompareTag("Player"))
        {
            // Choose a random index from the audioClips array
            int randomIndex = Random.Range(0, audioClips.Length);

            // Set the audio clip and volume level
            audioSource.clip = audioClips[randomIndex];
            audioSource.volume = volumeLevels[randomIndex];

            // Play the audio clip
            if (audioSource != null && !audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        // Check if the collider that exited is tagged as "Player"
        if (other.CompareTag("Player"))
        {
            // Start the fade out coroutine
            StartCoroutine(FadeOut());
        }
    }
    private IEnumerator FadeOut()
    {
        float startVolume = audioSource.volume;

        // Gradually decrease the volume over the fadeDuration
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            audioSource.volume = Mathf.Lerp(startVolume, 0, t / fadeDuration);
            yield return null;
        }

        // Ensure the volume is set to zero
        audioSource.volume = 0;

        // Stop the audio
        audioSource.Stop();
    }

}
