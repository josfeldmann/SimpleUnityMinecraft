using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : Grabbable
{
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform gunPoint;
    [SerializeField] private float bulletSpeed = 20f;

    public override void TriggerAction()
    {
        base.TriggerAction();
        Instantiate(bullet, gunPoint.position, gunPoint.rotation).GetComponent<Rigidbody>().AddForce(gunPoint.forward*bulletSpeed);
        //OVRInput.SetControllerVibration(1, 1, controller);

    }
}
