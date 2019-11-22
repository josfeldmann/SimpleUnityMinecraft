using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{

    public float currentHP, maxHP;


    private void Awake()
    {
        currentHP = maxHP;
    }


    public void TakeDamage(float damage)
    {
        currentHP -= damage;
        OnTakeDamage();
        if (currentHP <= 0) Die();
    }

    public void Die()
    {
        OnDeath();
        Destroy(gameObject);
    }

    public virtual void OnDeath() { }

    public virtual void OnTakeDamage()
    {

    }




}
