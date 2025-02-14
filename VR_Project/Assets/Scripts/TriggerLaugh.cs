using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TriggerLaugh : MonoBehaviour
{
    private AudioSource audioSource;
    public static event Action OnAudioTrigger; // Event that gets called when triggered

    private void Start()
    {
        // Get the AudioSource component on the same GameObject
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            Debug.LogError("No AudioSource found on " + gameObject.name);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object entering the trigger is the player
        if (other.CompareTag("Player") && audioSource != null)
        {
            if (!audioSource.isPlaying) // Prevent restarting if already playing
            {
                audioSource.Play();
                OnAudioTrigger?.Invoke(); // Notify all listeners
            }
        }
    }
}
