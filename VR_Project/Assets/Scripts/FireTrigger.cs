using UnityEngine;
using UnityEngine.SceneManagement;


public class FireTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Ensure only the player is affected
        {
            Debug.Log("Player touched the fire!");
            SceneManager.LoadScene("TestDoor 1");
            // Example: Apply damage or other effects


        }
    }
}
