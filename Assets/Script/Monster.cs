using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline.Actions;
using UnityEngine;
using UnityEngine.UI;

public class Monster : Creature
{
    public float speed = 0.1f;

    public Image HPUI;
    private Animator animator;
    private Rigidbody rb;
    private BoxCollider bc;
    private MainPlayer player = null;

    private float attackDelay = 3f;
    private float lastAttackTime;
    private bool move = true;


    // Start is called before the first frame update
    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        bc = GetComponent<BoxCollider>();
    }
    private void Start()
    {
        currentHealth = maxHealth;
        move = true;
        animator.SetBool("Move", true);
    }

    private void FixedUpdate()
    {
        if (move) MovePosition();
    }

    // Update is called once per frame  
    private void Update()
    {
        if(dead) return;

        Attack();
    }

    private void Attack()
    {
        if (player == null) return;
        if (lastAttackTime + attackDelay > Time.time) return;

        animator.SetTrigger("Attack");
        lastAttackTime = Time.time;
    }

    private void GiveDamage()
    {
        if (player == null) return;
        player.TakeDamage(damage);
    }

    private void Destroy()
    {
        Debug.Log("Á×À½");
        player.MonsterDie();
        Destroy(gameObject);
    }

    override public void TakeDamage(int damage)
    {
        base.TakeDamage(damage);

        HPUI.fillAmount = (float)currentHealth / maxHealth;
        if (dead)
        {
            animator.SetBool("Death", true);
        }
    }

    private void MovePosition()
    {
        Vector3 pos = rb.position + Vector3.left * speed;
        rb.MovePosition(pos);
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            player = collision.gameObject.GetComponent<MainPlayer>();
            animator.SetBool("Move", false);
            move = false;
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            player = null;
        }
    }
}
