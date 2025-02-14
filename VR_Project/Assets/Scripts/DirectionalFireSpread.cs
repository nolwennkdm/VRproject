using System.Collections;
using UnityEngine;

public class DirectionalFireSpread : MonoBehaviour
{
    public GameObject firePrefab; // Fire prefab to instantiate
    public float spreadInterval = 1f; // Time interval between each fire instance
    public float spreadDistance = 2f; // Distance between each fire instance
    public Vector3 spreadDirection = Vector3.forward; // Direction the fire spreads
    public float burnDuration = 10f; // Time before fire extinguishes
    public int damageAmount = 10; // Damage to apply when the player touches fire

    private void Start()
    {
        StartCoroutine(SpreadFire());
    }

    private IEnumerator SpreadFire()
    {
        float elapsedTime = 0f;

        while (elapsedTime < burnDuration)
        {
            Vector3 spawnPosition = transform.position + spreadDirection.normalized * spreadDistance * (elapsedTime / spreadInterval);

            // Instancier le feu
            GameObject newFire = Instantiate(firePrefab, spawnPosition, Quaternion.identity);

            // Vérifier si un Collider est présent, sinon l'ajouter
            if (newFire.GetComponent<Collider>() == null)
            {
                BoxCollider collider = newFire.AddComponent<BoxCollider>();
                collider.isTrigger = true; // Si on veut détecter le joueur sans collision physique
            }

            newFire.AddComponent<FireTrigger>(); // Ajoute la détection du joueur

            yield return new WaitForSeconds(spreadInterval);
            elapsedTime += spreadInterval;
        }
    }
}