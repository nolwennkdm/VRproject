using UnityEngine;
using UnityEngine.SceneManagement;

public class AnomalieManager : MonoBehaviour
{
    public int erreursMax = 7; // Nombre de réussites nécessaires
    private int succesActuels = 0; // Nombre de succès consécutifs
    private string sceneBase = "SceneDeBase"; // Nom de la scène de base
    /// Vecteur pour la position fixe du joueur lors du changement de scène 
    private Vector3 fixedSpawnPosition = new Vector3(-1.88f, 1.47f, 37.39f); 
    
// Rotation fixe (0, 0, 0) 
    private Quaternion fixedRotation = Quaternion.Euler(0f, 0f, 0f);

    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        // Optionnel: Charger l'état de l'anomalie sauvegardé précédemment
        // anomalieActive = PlayerPrefs.GetInt("AnomalieActive", 0) == 1;
    }

    void Awake()
    {
        DontDestroyOnLoad(gameObject); // Garde cet objet même après un changement de scène

        // Empêcher la duplication de l'objet
        if (FindObjectsOfType<AnomalieManager>().Length > 1)
        {
            Destroy(gameObject);
            return;
        }
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) { 
        // Réinitialiser la position et la rotation de XR Origin directement (car le script est attaché à XR Origin) 
        transform.position = fixedSpawnPosition; 
        transform.rotation = fixedRotation; 
        // Réinitialiser la position et la rotation de tous les enfants (y compris les sous-enfants)
       } 
        // Fonction récursive pour réinitialiser la position et la rotation de tous les enfants et sous-enfants private 
   

    // Méthode appelée quand le joueur entre dans un trigger
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collision détectée avec : " + other.gameObject.name);

        // Vérifier si l'objet touché est une porte
        if (!other.gameObject.CompareTag("anomalie") && !other.gameObject.CompareTag("sansAnomalie"))
        {
            Debug.Log("L'objet touché n'est pas une porte, on ignore.");
            return;
        }

        // Récupérer le nom de la scène actuelle
        string sceneActuelle = SceneManager.GetActiveScene().name;
        bool isBaseScene = sceneActuelle == sceneBase;

        Debug.Log("Le joueur touche une porte de type : " + other.gameObject.tag);

        // Vérification si c'est la bonne porte selon la scène actuelle
        if ((isBaseScene && other.gameObject.CompareTag("sansAnomalie")) ||
            (!isBaseScene && other.gameObject.CompareTag("anomalie")))
        {
            succesActuels++;
            Debug.Log("Succès : " + succesActuels);

            if (succesActuels >= erreursMax)
            {
                GagnerLeJeu();
            }
            else
            {
                LancerSceneSuivante();
            }
        }
        else
        {
            Debug.Log("Mauvais choix ! Reset.");
            Recommencer();
        }
    }

    // Réinitialise le jeu en cas de mauvais choix
    void Recommencer()
    {
        succesActuels = 0;
        // Recharger la scène de base
        SceneManager.LoadScene(sceneBase);
    }

    // Gagner le jeu si on atteint le nombre d'essais maximum
    void GagnerLeJeu()
    {
        Debug.Log("Félicitations ! Vous avez gagné après " + erreursMax + " succès d'affilée !");
        // Optionnellement, charger une scène de victoire
        // SceneManager.LoadScene("SceneDeVictoire");
    }

    // Lance la scène suivante avec une probabilité pour activer l'anomalie
    void LancerSceneSuivante()
    {
        float probabilité = Random.Range(0f, 1f);

        // 10% de chance de rester en mode normal, 90% de chance d'entrer en anomalie
        if (probabilité < 0.001f)
        {
            SceneManager.LoadScene(sceneBase);
        }
        else
        {
            int anomalieNum = Random.Range(1, 5); // Générer un nombre entre 1 et 4
            SceneManager.LoadScene("Anom4");
        }
    }
    void OnDestroy() { // Se désabonner de l'événement lors de la destruction de l'objet 
        SceneManager.sceneLoaded -= OnSceneLoaded; }
}
