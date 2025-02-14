using System; // Needed for Action events
using System.Collections; 
using UnityEngine;


public class AudioListener : MonoBehaviour
{
    private AudioSource audioSource;
    public float delayTime = 2f; // Time to wait before playing sound

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        TriggerLaugh.OnAudioTrigger += DelayedPlaySound; // Subscribe to event
    }

    private void OnDestroy()
    {
        TriggerLaugh.OnAudioTrigger -= DelayedPlaySound; // Unsubscribe to prevent memory leaks
    }

    private void DelayedPlaySound()
    {
        if (!audioSource.isPlaying)
        {
            StartCoroutine(PlaySoundWithDelay());
        }
    }

    private IEnumerator PlaySoundWithDelay()
    {
        yield return new WaitForSeconds(delayTime); // Wait for delayTime seconds
        audioSource.Play();
    }
}
