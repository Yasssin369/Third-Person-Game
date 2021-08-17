using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public float health = 100f;
    public int damage = 20;
    [SerializeField] int points = 100;

    public Animator animator;
    public GameObject player;
    public ParticleSystem bloodParticle;
    public void Hit(float damage)
    {
        if(health >=0)
        { 
        health -= damage;
        bloodParticle.Emit(100);
        }
        if (health <= 0)
        {

            animator.SetTrigger("Death");
            GetComponent<NavMeshAgent>().isStopped = true;
            GetComponent<NavMeshAgent>().velocity = Vector3.zero;

        }
    }
    public void RemoveDead(float health)
    {
        if  (health <= 0)
        {
            Destroy(gameObject);
        }
    }
    public void Score()
    {
        ScoreSystem.Add(points);

    }
    void Start()
    {
        animator=GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
        bloodParticle = GetComponentInChildren<ParticleSystem>();
    }

    private void FixedUpdate()
    {
        if (health > 0)
        {
            GetComponent<NavMeshAgent>().destination = player.transform.position;
            if (GetComponent<NavMeshAgent>().velocity.magnitude > 1)
            {
                animator.SetBool("isRunning", true);
            }
            if (GetComponent<NavMeshAgent>().remainingDistance <= GetComponent<NavMeshAgent>().stoppingDistance + 0.2f)
            {
                animator.SetBool("Attack", true);
            }
            else
            {
                animator.SetBool("Attack", false);
            }
        }
    }
    /*private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            Debug.Log("deaaaad biaaatch");
            player.GetComponent<ThirdPersonMovement>().Damage(damage);
        }
    }*/

    private void OnCollisionEnter(Collision collision)
    {   
        if(collision.gameObject == player)
        {
            player.GetComponent<ThirdPersonMovement>().Damage(damage);
        }

    }
}
