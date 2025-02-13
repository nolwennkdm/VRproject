using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BreakGlass : MonoBehaviour
{
    // Start is called before the first frame update
    private AudioSource glassAudio;

    void Awake()
    {
        glassAudio = GetComponent<AudioSource>(); // Initialize the audio source
    }
    void PlayAudioDetached()
    {
        // Create a new GameObject to play the sound
        GameObject audioObject = new GameObject("GlassSound");
        AudioSource audioSource = audioObject.AddComponent<AudioSource>();
        audioSource.clip = glassAudio.clip;
        audioSource.volume = glassAudio.volume;
        audioSource.pitch = glassAudio.pitch;
        audioSource.spatialBlend = glassAudio.spatialBlend; // If it's a 3D sound
        audioSource.Play();

        // Destroy the new GameObject after the sound finishes
        Destroy(audioObject, audioSource.clip.length);
    }
    private void OnTriggerEnter(Collider other)
    {
        // Vérifie si l'objet qui entre est bien le marteau (tag "Hammer")
        if (other.CompareTag("Hammer"))
        {
            // Désactive l'objet actuel
            Debug.Log("Collided with: " + other.gameObject.name);
            if (glassAudio != null)
            {
                PlayAudioDetached();
            }
            Destroy(gameObject);
        }
    }
}
