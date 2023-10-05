using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class Creature : MonoBehaviour
{
    protected int maxHealth = 100;
    protected int currentHealth = 0;
    protected int damage = 30;
    protected bool dead = false;

    protected Action onDeath;

    // Start is called before the first frame update

    virtual public void TakeDamage(int damage)
    {
        if (dead) return;

        currentHealth -= damage;
        if(currentHealth <= 0)
        {
            currentHealth = 0;
            //onDeath();
            dead = true;
        }
    }
}
