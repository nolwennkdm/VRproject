using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class HammerGrab : MonoBehaviour
{
    private XRGrabInteractable grabInteractable;
    private Rigidbody rb;

    private void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        rb = GetComponent<Rigidbody>();

        // Subscribe to XR grab events
        grabInteractable.selectEntered.AddListener(OnGrab);
        grabInteractable.selectExited.AddListener(OnRelease);
    }

    private void OnGrab(SelectEnterEventArgs args)
    {
        rb.isKinematic = true; // Make kinematic when grabbed
    }

    private void OnRelease(SelectExitEventArgs args)
    {
        rb.isKinematic = false; // Enable physics when released
    }
}
