using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : MonoBehaviour
{

    protected ControllerInput controller;
    public void Awake()
    {
        controller = transform.parent.GetComponent<ControllerInput>();
    }



}
