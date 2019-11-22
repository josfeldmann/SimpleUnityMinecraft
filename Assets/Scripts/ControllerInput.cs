using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(OculusHaptics))]
public class ControllerInput : MonoBehaviour
{

    public OVRInput.Controller controllerType;
    public Vector2 thumbstickInput;
    public bool gripTriggerDown, gripTriggerUp, mainTriggerDown, secondaryTriggerDown, buttonOne, buttonOneUp, buttonTwoDown, menuButtonDown;
    private  OculusHaptics haptics;


    public void VibrateTime(VibrationForce force, float time)
    {
        haptics.VibrateTime(force, time);
    }

    public void Awake()
    {
        haptics = GetComponent<OculusHaptics>();
        haptics.controllerMask = controllerType;
    }

    public void GetInputs()
    {



        thumbstickInput = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, controllerType);// + new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        gripTriggerDown = OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger, controllerType);// || Input.GetMouseButtonDown(0);
        gripTriggerUp = OVRInput.GetUp(OVRInput.Button.PrimaryHandTrigger, controllerType);// || Input.GetMouseButtonUp(0);
        mainTriggerDown = OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger, controllerType);
        buttonOne = OVRInput.Get(OVRInput.Button.One, controllerType);
        buttonOneUp = OVRInput.GetUp(OVRInput.Button.One, controllerType);
        buttonTwoDown = OVRInput.GetDown(OVRInput.Button.Two, controllerType);// || Input.GetKeyDown(KeyCode.Space);
        menuButtonDown = OVRInput.GetDown(OVRInput.Button.Start, controllerType);


    }

    public void Update()
    {
        GetInputs();
    }




}
