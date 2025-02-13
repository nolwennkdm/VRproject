using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class MovePlayerVR : MonoBehaviour
{
    public float moveSpeed = 3f;  // Vitesse de d�placement
   

    private CharacterController characterXRController;  // Contr�leur de personnage
    private InputDevice leftXRController; // Manette gauche
    private InputDevice rightXRController; // Manette droite
    private Vector3 moveDirection; // Direction de mouvement

    void Start()
    {
        characterXRController = GetComponent<CharacterController>();

        // R�cup�rer les contr�leurs VR (selon votre mat�riel)
        var leftHand = new List<InputDevice>();
        var rightHand = new List<InputDevice>();

        InputDevices.GetDevicesAtXRNode(XRNode.LeftHand, leftHand);
        InputDevices.GetDevicesAtXRNode(XRNode.RightHand, rightHand);

        if (leftHand.Count > 0)
            leftXRController = leftHand[0];

        if (rightHand.Count > 0)
            rightXRController = rightHand[0];
    }

    void Update()
    {
        Vector2 left = Vector2.zero;
        if (leftXRController.TryGetFeatureValue(CommonUsages.primary2DAxis, out left))
        {
            if (left.magnitude > 0.1f)
            {
                Vector3 forward = transform.forward * left.y;
                Vector3 right = transform.right * left.x;
                moveDirection = (forward + right) * moveSpeed;
            }
            else
            {
                moveDirection = Vector3.zero;
            }




            characterXRController.Move(moveDirection * Time.deltaTime);
        }
    }
}