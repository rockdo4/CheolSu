using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class Creature : MonoBehaviour
{
    public int MaxHealth { get; set; } = 10;
    public int currentHealth = 0;
    public int Damage { get; set; } = 2;
    public bool dead = false;
    public Transform popPos;

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

    public void PopDamage(int damage, Color color, Vector3 position)
    {
        var obj = ObjectPoolManager.instance.GetGo("PopDamage");
        obj.transform.position = popPos.position;
        obj.GetComponent<PopDamage>().damage.SetText($"{damage}");
    }
}
