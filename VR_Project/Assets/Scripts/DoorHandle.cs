using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class DoorHandle : MonoBehaviour
{
    public Transform door; // Reference to the door pivot
    public float rotationSpeed = 100f; // Rotation speed
    private XRGrabInteractable grabInteractable;
    private Quaternion initialRotation;

    private void Start()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        grabInteractable.selectExited.AddListener(OnRelease); // Fixed event listener
        initialRotation = transform.localRotation;
    }

    private void Update()
    {
        if (grabInteractable.isSelected)
        {
            // Get handle's local rotation relative to its initial state
            Quaternion relativeRotation = Quaternion.Inverse(initialRotation) * transform.localRotation;

            // Apply only Y-axis rotation to the door
            float rotationY = relativeRotation.eulerAngles.y;
            door.localRotation = Quaternion.Euler(0, rotationY, 0);
        }
    }

    private void OnRelease(SelectExitEventArgs args) // Fixed parameter type
    {
        StartCoroutine(ResetHandle());
    }

    private IEnumerator ResetHandle()
    {
        Quaternion startRotation = transform.localRotation;
        float elapsedTime = 0f;
        float duration = 0.2f; // Smooth return duration

        while (elapsedTime < duration)
        {
            transform.localRotation = Quaternion.Slerp(startRotation, initialRotation, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.localRotation = initialRotation;
    }
}
