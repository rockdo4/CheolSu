using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainPlayer : Creature
{
    public Image HPUI;

    private float attackDelay = 2f;
    private float lastAttackTime;

    private Animator animator;
    private Creature enemy = null;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        currentHealth = maxHealth;

        animator.SetBool("Move", true);
    }

    // Update is called once per frame
    private void Update()
    {
        if (dead) return;

        Attack();
    }

    private void Attack()
    {
        if (enemy == null) return;
        if (lastAttackTime + attackDelay > Time.time) return;

        animator.SetTrigger("Attack");
        lastAttackTime = Time.time;
        Debug.Log(Time.time);
    }

    private void GiveDamage()
    {
        if(enemy == null) return;
        enemy.TakeDamage(damage);
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            animator.SetBool("Move", false);
            enemy = collision.gameObject.GetComponent<Monster>();
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if(collision.CompareTag("Enemy"))
        {
            animator.SetBool("Move", true);
            enemy = null;
        }
    }

    public void MonsterDie()
    {
        Invoke("StartMove", 0.5f);
    }

    private void StartMove()
    {
        animator.SetBool("Move", true);
    }

    override public void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        if (dead)
        {
            animator.SetBool("Death", true);
        }
        HPUI.fillAmount = (float)currentHealth / maxHealth;
    }
}
