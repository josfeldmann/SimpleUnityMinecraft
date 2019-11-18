using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/*
 * This class extends Monobehaviour which means it has access to monobehaviour methods and can be
 * added onto a gameobject in the inspector. All script components that are attached gameobjects
 * will extend Monobehaviour at some point.
 */
public class PlayerController : MonoBehaviour {


    /*
     *    These are public variables, meaning other scripts and classes will be able to access these fields.
     *    Making Monobehaviour variables public will also allow them to be viewed and initialized in the inspector.
     *    This allows us to easily change varables without having to change around the actual scripts whenever we want
     *    to tweak a float value.
     */
    
    //This is a reference to the main camera which we will rotate and use for movement later
    public Camera cam;

    //These are all floats that control camera movement
    public float xSensitivity, ySensitivity, maxXRot;

    public float speed = 6.0f;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;

    private Vector3 moveDirection = Vector3.zero;
    private CharacterController controller;
    
    void Start () {
        controller = GetComponent<CharacterController>();
    }
	
	void FixedUpdate () {

       

        if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0) {

              float roty, rotx;
              rotx = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * xSensitivity;
              roty = cam.transform.localEulerAngles.x - Input.GetAxis("Mouse Y") * ySensitivity;
              transform.localEulerAngles = new Vector3(transform.eulerAngles.x, rotx, transform.eulerAngles.z);
              cam.transform.localEulerAngles = new Vector3(roty,0,0);

        }


        if (controller.isGrounded) {
            // We are grounded, so recalculate
            // move direction directly from axes

            
            moveDirection = (cam.transform.forward * Input.GetAxis("Vertical")) + (cam.transform.right * Input.GetAxis("Horizontal"));
            moveDirection.y = 0;
            moveDirection = moveDirection * speed;

            if (Input.GetButton("Jump")) {
                moveDirection.y = jumpSpeed;
            }
        }

        // Apply gravity
        moveDirection.y = moveDirection.y - (gravity * Time.deltaTime);

        // Move the controller
        controller.Move(moveDirection * Time.deltaTime);



    }
}
