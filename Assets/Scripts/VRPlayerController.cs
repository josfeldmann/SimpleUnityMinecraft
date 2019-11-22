using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VRPlayerController : Unit
{

    [SerializeField]private ControllerInput primaryController, secondaryController;

    [SerializeField] private CharacterController characterController;

    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private Camera cam;
    [SerializeField] private float jumpSpeed = 5f;
    [SerializeField] private float gravity = 9.8f;

    [SerializeField] private bool useMouseKeyboard;
    [SerializeField] private float camSpeed;
    [SerializeField] private Transform camBody;
    [SerializeField] private VRPlayerUIManager uiManager;
    public Image mapImage;
    private bool teleporting;

    //Local Variables
    private Vector3 move;
    private Vector3 spawnPosition;
    private float vSpeed;
    private Vector3 offset;
    private float estimatedTime;


    // Start is called before the first frame update
    void Start()
    {
        // characterController = GetComponent<CharacterController>();
        spawnPosition = transform.position;
    }

 
  
    void Update()
    {
        if (!teleporting)
        {

            move = (primaryController.thumbstickInput +
                secondaryController.thumbstickInput +
                (new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")))).normalized;

            move = cam.transform.forward * (move.y) +
                   cam.transform.right * (move.x);
            move *= moveSpeed;
            move.y = 0;

            characterController.enabled = false;
            characterController.transform.position= new Vector3(cam.transform.position.x,
                                                                 characterController.transform.position.y,
                                                                 cam.transform.position.z);
            characterController.enabled = true;

            if (characterController.isGrounded)
            {
                vSpeed = 0; // grounded character has vSpeed = 0...
                if (secondaryController.buttonTwoDown || primaryController.buttonTwoDown)
                { // unless it jumps:
                    vSpeed = jumpSpeed;
                }
            }
            // apply gravity acceleration to vertical speed:
            vSpeed -= gravity * Time.deltaTime;

            offset = camBody.position - characterController.transform.position;


            move.y = vSpeed;

            //   transform.position = new Vector3(playerCamera.transform.position.x, transform.position.y, playerCamera.transform.position.z);


           characterController.Move(move * Time.deltaTime);
           characterController.transform.rotation =  Quaternion.Euler(0,cam.transform.rotation.eulerAngles.y,0);

            camBody.position = characterController.transform.position + offset;


        }

            if (characterController.transform.position.y < -100) {
            MoveToGlobalPosition(spawnPosition);
            TakeDamage(1);
           }




                if (useMouseKeyboard)
        {
            //cam.transform.Rotate(Input.GetAxis("Mouse Y") * camSpeed * Time.deltaTime, 0, 0);
           // transform.Rotate(0, Input.GetAxis("Mouse X") * camSpeed * Time.deltaTime, 0);
        }
        

    }


    public void DashTeleport(Vector3 pos, float metersPerSecond)
    {
        if (!teleporting)StartCoroutine(DashTeleportCoroutine(pos, metersPerSecond));
    }

    public void BlinkTeleport(Vector3 pos)
    {
        if (!teleporting) StartCoroutine(BlinkTeleportCoroutine(pos));
    }

    private IEnumerator BlinkTeleportCoroutine(Vector3 pos)
    {

        yield return null;
        MoveToGlobalPosition(pos);

    }


  
    private IEnumerator DashTeleportCoroutine(Vector3 targetLocation, float metersPerSecond)
    {
        teleporting = true;

        estimatedTime = Vector3.Distance(characterController.transform.position , targetLocation)/metersPerSecond;
        characterController.enabled = false;
        while (estimatedTime > 0)
        {
            estimatedTime -= Time.deltaTime;

            offset = camBody.position - characterController.transform.position;
            characterController.transform.position = Vector3.MoveTowards(characterController.transform.position, targetLocation, metersPerSecond * Time.deltaTime);
            camBody.position = characterController.transform.position + offset;
            yield return null;

        }
        targetLocation.y = characterController.transform.position.y;
        MoveToGlobalPosition(targetLocation);

        print("TeleportEnded");

        teleporting = false;
    }

    public override void OnTakeDamage()
    {
        base.OnTakeDamage();
        uiManager.UpdateDamageVisual(currentHP, maxHP, 0, 0);
    }


    public void MoveToGlobalPosition(Vector3 targetLocation)
    {
        characterController.enabled = false;
        offset = camBody.position - characterController.transform.position;
        characterController.transform.position = new Vector3(targetLocation.x, targetLocation.y, targetLocation.z);
        camBody.position = characterController.transform.position + offset;
        characterController.enabled = true;
    }

}
