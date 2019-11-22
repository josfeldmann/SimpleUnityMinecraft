using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    public float timeToDestroy = 10f;
    public float baseDamage;
    public AudioClip hitSound;

    void Start()
    {
        Destroy(gameObject, timeToDestroy);
    }

    public void init()
    {

    }


    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnCollisionEnter(Collision collision)
    {


        OnHit(collision);

       

        
    }

    public void OnHit(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponent<Unit>().TakeDamage(baseDamage);
            
            Destroy(gameObject);
        }
        AudioSource.PlayClipAtPoint(hitSound, transform.position);
    }

    


}
