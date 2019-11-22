using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : Unit
{


    [SerializeField] private List<Transform> targets = new List<Transform>();
    [SerializeField] private Transform currentTarget;
    [SerializeField] private Transform turretHead;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (currentTarget != null )turretHead.LookAt(currentTarget, Vector3.up);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            targets.Add(other.transform);
            if (currentTarget == null) currentTarget = other.transform;
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            targets.Remove(other.transform);
            if (currentTarget == other.transform)
            {
                if (targets.Count > 0) currentTarget = targets[0];
                else currentTarget = null;
            }
        }
    }
}
