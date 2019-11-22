using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OVR;

public class Grabbable : MonoBehaviour
{
  
    public Vector3 defaultPosition, defaultRotation;
    public Rigidbody rb;
    protected OVRInput.Controller controller;

    public void Awake()
    {
        transform.SetParent(null);
    }

    public void AttachTo(OVRInput.Controller e, Transform t)
    {

        controller = e;
        transform.SetParent(t);
        rb.isKinematic = true;
        rb.angularVelocity = Vector3.zero;
        rb.velocity = Vector3.zero;
        rb.interpolation = RigidbodyInterpolation.None;
        transform.localPosition = defaultPosition;
        transform.localRotation = Quaternion.Euler(defaultRotation);
        
    }

    public void Detach(){
        transform.SetParent(null);
        rb.isKinematic = false;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.velocity = OVRInput.GetLocalControllerVelocity(controller);
        rb.angularVelocity = OVRInput.GetLocalControllerAngularVelocity(controller);

        
    }

    public virtual void TriggerAction()
    {

    }




}
