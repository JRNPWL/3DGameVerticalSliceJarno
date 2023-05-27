using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public float damage = 10f;
    public float health { get; private set; } = 100f;
    public event System.Action<GameObject> OnDeath;
    public int scoreValue;

    [SerializeField] private float attackRate = 1f;

    private float lastAttackTime = -Mathf.Infinity;
    private Animator animator;
    private NavMeshAgent navMeshAgent;
    private CapsuleCollider capsuleCollider;
    private new Rigidbody rigidbody;

    public GameObject zombieModel;

    Transform playerTransform;
    void Start()
    {
        // Find Player Object
        GameObject player = GameObject.Find("PlayerCapsule");
        playerTransform = player.GetComponent<Transform>();

        navMeshAgent = GetComponent<NavMeshAgent>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // If NavMesh is Active and Enemey is On Mesh -> Go to Player
        if (navMeshAgent.isActiveAndEnabled)
        {
            if (navMeshAgent.isOnNavMesh)
            {
                transform.LookAt(playerTransform.transform);
                navMeshAgent.SetDestination(playerTransform.position);
            }
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (Time.time - lastAttackTime >= attackRate)
            {

                // Randomize Attack Animation
                int randomValue = Random.Range(0, 3);
                Debug.Log($"Attack Animation: {randomValue}");
                animator.SetTrigger("Attack" + randomValue);

                // Damages The Player
                collision.gameObject.GetComponent<PlayerHealth>().TakeDamage(damage);
                lastAttackTime = Time.time;
            }
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;

        int randomValue = Random.Range(0, 2);
        Debug.Log($"Hit Animation randomValue: {randomValue}");
        animator.SetTrigger("Hit" + 1);

        // Check if the enemy's health is less than or equal to 0
        if (health <= 0)
        {
            if (OnDeath != null)
            {
                OnDeath(gameObject);
            }
        }
    }

    public void ResetEnemy()
    {
        navMeshAgent.enabled = true;
        capsuleCollider.enabled = true;
        rigidbody.isKinematic = false;
    }
}