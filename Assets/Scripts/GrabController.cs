using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using TMPro;

[System.Serializable]
public enum ControllerEnum { RIGHT, LEFT}


[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(SphereCollider))]
public class GrabController : HandController
{
    // Start is called before the first frame update
   
    public List<Grabbable> heldGrabbables = new List<Grabbable>();
    public Grabbable heldItem;
    private InputDevice device;
    [SerializeField] private float grabThreshold = 0.7f;
    [SerializeField] private TextMeshProUGUI debugText;


    

    public void OnTriggerEnter(Collider other) {
        
        if (other.transform.tag == "Grab"){
            print(gameObject.name + " Entered Over: " + other.gameObject.name);
            heldGrabbables.Add(other.gameObject.GetComponent<Grabbable>());
           // OVRInput.SetControllerVibration( 0.5f, 0.5f, controller.controllerType);
        }
    }

    private void OnTriggerExit(Collider other) {
        
        if (other.transform.tag == "Grab"){
            print(gameObject.name + " Exited: " + other.gameObject.name);

            heldGrabbables.Remove(other.gameObject.GetComponent<Grabbable>());
        }
    }


    public void Update(){

        
       
        if (controller.gripTriggerDown){
            print( gameObject.name +" MainTriggerDown");
          
                if (heldGrabbables.Count == 0) return;
                heldItem = heldGrabbables[0];
                heldItem.AttachTo(controller.controllerType, transform);
                controller.VibrateTime(VibrationForce.Light,0.1f);
        }

        if (controller.gripTriggerUp)
        {
            print(gameObject.name + " MainTriggerUp");
            if (heldItem)
            {
                heldItem.Detach();
                heldItem = null;
            }
            
        }

        if (controller.mainTriggerDown && heldItem)
        {
            heldItem.TriggerAction();
        }
        


    }



}
