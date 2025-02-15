using UnityEngine;
using UnityEngine.SceneManagement;

public class AnomalieManager : MonoBehaviour
{
    public int erreursMax = 7; // Nombre de réussites nécessaires
    private int succesActuels = 0; // Nombre de succès consécutifs
    private string sceneBase = "SceneDeBase"; // Nom de la scène de base
    private Vector3 fixedSpawnPosition = new Vector3(-1.88f, 1.47f, 37.39f);
    private Quaternion fixedRotation = Quaternion.Euler(0f, 0f, 0f);
    private Transform originalParent; // Référence au parent original

    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void Awake()
    {
        // Conserver la référence au parent original
        originalParent = transform.parent;

        // Empêcher la duplication de l'objet
        if (FindObjectsOfType<AnomalieManager>().Length > 1)
        {
            Destroy(gameObject);
            return;
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Scene loaded: " + scene.name);
        Debug.Log("AnomalieManager is active: " + gameObject.activeSelf);

        // Réinitialiser la position et la rotation de XR Origin directement (car le script est attaché à XR Origin)
        transform.position = fixedSpawnPosition;
        transform.rotation = fixedRotation;

        // Réattacher l'objet à son parent original si possible
        if (originalParent != null)
        {
            transform.SetParent(originalParent);
        }
        else
        {
            // Si le parent original n'existe plus, trouver un nouveau parent (par exemple, le joueur)
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                transform.SetParent(player.transform);
            }
        }

        // Réinitialiser la position et la rotation de tous les enfants (y compris les sous-enfants)
    }

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

    public void Recommencer()
    {
        succesActuels = 0;

        // Détacher l'objet de son parent avant de recharger la scène
        transform.SetParent(null);
        DontDestroyOnLoad(gameObject);

        // Recharger la scène de base
        SceneManager.LoadScene(sceneBase);
    }

    void GagnerLeJeu()
    {
        Debug.Log("Félicitations ! Vous avez gagné après " + erreursMax + " succès d'affilée !");
        
        SceneManager.LoadScene("Win");
    }

    void LancerSceneSuivante()
    {
        float probabilité = Random.Range(0f, 1f);

        // Détacher l'objet de son parent avant de charger la nouvelle scène
        transform.SetParent(null);
        DontDestroyOnLoad(gameObject);

        // 10% de chance de rester en mode normal, 90% de chance d'entrer en anomalie
        if (probabilité < 0.25f)
        {
            SceneManager.LoadScene(sceneBase);
        }
        else
        {
            int anomalieNum = Random.Range(1, 9); // Générer un nombre entre 1 et 8
            SceneManager.LoadScene("Anom"+anomalieNum);
        }
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
