using UnityEngine;

public class GazeChangeMaterial : MonoBehaviour
{
    public Material gazeMaterial; // Nouveau mat�riau quand on regarde l'objet
    private Material originalMaterial;
    private Renderer objRenderer;
    private Transform xrHead; // R�f�rence au casque VR

    private void Start()
    {
        objRenderer = GetComponent<Renderer>();
        if (objRenderer != null)
        {
            originalMaterial = objRenderer.material;
        }

        // Assigne le casque VR (le head-mounted display)
        xrHead = Camera.main?.transform;
    }

    private void Update()
    {
        if (xrHead == null) return;

        // Lancer un Raycast depuis le regard de l'utilisateur
        Ray gazeRay = new Ray(xrHead.position, xrHead.forward);
        RaycastHit hit;

        if (Physics.Raycast(gazeRay, out hit, Mathf.Infinity))
        {
            if (hit.transform == transform)
            {
                ChangeMaterial(gazeMaterial); // Appliquer le mat�riau si l'objet est regard�
            }
            else
            {
                ChangeMaterial(originalMaterial);
            }
        }
        else
        {
            ChangeMaterial(originalMaterial);
        }
    }

    private void ChangeMaterial(Material newMat)
    {
        if (objRenderer.material != newMat)
        {
            objRenderer.material = newMat;
        }
    }
}
