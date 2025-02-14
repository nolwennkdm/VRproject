using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class remy : MonoBehaviour
{
    public Transform player; // Référence au transform du personnage
    public Transform targetPosition; // Position cible que le personnage doit atteindre
    public Animator animator; // Référence à l'Animator du PNJ
    public float detectionRadius = 5f; // Rayon de détection autour de la position cible

    private bool isPlayerDetected = false;

    void Update()
    {
        // Vérifie si le personnage est à proximité de la position cible
        if (Vector3.Distance(player.position, targetPosition.position) < detectionRadius && !isPlayerDetected)
        {
            isPlayerDetected = true;
            animator.SetTrigger("Stand"); // Déclenche l'animation de lever
        }

        // Si le personnage a été détecté, le PNJ le suit du regard
        if (isPlayerDetected)
        {
            Vector3 lookDirection = player.position - transform.position;
            lookDirection.y = 0; // Pour éviter que le PNJ ne regarde vers le haut ou le bas
            transform.LookAt(transform.position + lookDirection);
        }
    }
}
