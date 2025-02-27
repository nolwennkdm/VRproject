using UnityEngine;
using System.Collections;

public class Monster : MonoBehaviour
{
    public GameObject breakableObject;  // Objet destructible
    public Transform player;  // Référence au joueur
    public float jumpForce = 5f;
    public float crawlSpeed = 2f;
    public float attackDistance = 1f;
    public float attackCooldown = 5f;
    public ParticleSystem ps; // Effet de sang

    private Rigidbody rb;
    private Animator anim;
    private bool isCrawling = false;
    private bool canAttack = true;
    private AudioSource audioSource;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        

        if (audioSource == null)
        {
            Debug.LogError("No AudioSource found on " + gameObject.name);
        }
    }

    void Update()
    {
        // Vérifie si l'objet cassable a été détruit
        if (breakableObject == null && !isCrawling)
        {
            Jump(); // Déclenche le saut
            if (!audioSource.isPlaying) // Prevent restarting if already playing
            {
                audioSource.Play();
                // Notify all listeners
            }
        }

        if (isCrawling)
        {
            MoveTowardsPlayer();
        }
    }

    private void Jump()
    {
        anim.SetTrigger("jump");
        Vector3 jumpDirection = (player.position - transform.position).normalized; // Saut en direction du joueur
        rb.velocity = new Vector3(jumpDirection.x * jumpForce, jumpForce, jumpDirection.z * jumpForce); // Saut vers l'avant en 3D
        StartCoroutine(StartCrawlingAfterJump());
    }

    private IEnumerator StartCrawlingAfterJump()
    {
        yield return new WaitForSeconds(0.5f); // Temps du saut
        anim.SetTrigger("crawl");
        isCrawling = true;
    }

    private void MoveTowardsPlayer()
    {
        if (player != null)
        {
            Vector3 direction = (player.position - transform.position).normalized;
            rb.velocity = new Vector3(direction.x * crawlSpeed, rb.velocity.y, direction.z * crawlSpeed); // Déplacement en 3D
            transform.LookAt(player); // Oriente le monstre vers le joueur

            if (Vector3.Distance(transform.position, player.position) <= attackDistance && canAttack)
            {
                StartCoroutine(AttackPlayer());
            }
        }
    }

    private IEnumerator AttackPlayer()
    {
        canAttack = false;
        anim.SetTrigger("bite");

        // Déclencher l'effet de sang sur le joueur (optionnel)
        

        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
        AnomalieManager anomalieManager = FindObjectOfType<AnomalieManager>();
        anomalieManager.Recommencer();
    }

    // Quand le marteau touche le monstre, afficher du sang
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hammer"))
        {
            if (ps)
            {
                ps.Play();
            }
            anim.SetTrigger("dead");
        }
    }
}
