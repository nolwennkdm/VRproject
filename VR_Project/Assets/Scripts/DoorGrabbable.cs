using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class DoorGrabbable : XRGrabInteractable
    

{
    public Transform handler;

    protected override void OnSelectExited(SelectExitEventArgs interactor)
    {
        base.OnSelectExited(interactor);
        transform.position = handler.transform.position;
        transform.rotation = handler.transform.rotation;
        Rigidbody rbhandler = handler.GetComponent<Rigidbody>();
        rbhandler.velocity = Vector3.zero;
        rbhandler.angularVelocity = Vector3.zero;

    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    
}

