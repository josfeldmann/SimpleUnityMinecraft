using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeLauncher : Grabbable
{
    [SerializeField] private GameObject grenadeBullet;
    [SerializeField] private Transform gunPoint;
    [SerializeField] private float bulletSpeed = 20f;
    [SerializeField] private float upwardForce = 5f;

    public override void TriggerAction()
    {
        base.TriggerAction();
        Instantiate(grenadeBullet, gunPoint.position, gunPoint.rotation).GetComponent<Rigidbody>().AddForce((gunPoint.forward * bulletSpeed) + gunPoint.up * upwardForce);
        //OVRInput.SetControllerVibration(1, 1, controller);

    }
}
