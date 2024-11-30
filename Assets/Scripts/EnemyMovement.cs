using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CharacterController))]
public class EnemyMovement : MonoBehaviour
{
    public float speed = 1f;
    public float minDistance = 1.5f;
    public Transform target;
    public int damage = 1;
    public int health = 10; // Add health for the enemy
    public float attackCooldown = 1f; // Time between attacks

    private NavMeshAgent agent;
    private Animator animator;
    private CharacterController characterController;
    private bool isAttacking = false; // To check if the zombie is currently attacking
    private AudioSource idleMoan;
    private AudioSource attackSound;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        idleMoan = GetComponents<AudioSource>()[0];
        attackSound = GetComponents<AudioSource>()[1];
        animator.Play("Z_Idle");

        // Set the speed and stopping distance for the NavMeshAgent
        agent.speed = speed;
        agent.stoppingDistance = minDistance;

        // If no target is set, assume it is the player
        if (target == null)
        {
            if (GameObject.FindWithTag("Player") != null)
            {
                target = GameObject.FindWithTag("Player").GetComponent<Transform>();
            }
        }
    }

    void Update()
    {
        if (target == null) return;

        float distance = Vector3.Distance(transform.position, target.position);

        if (distance > minDistance)
        {
            agent.SetDestination(target.position);
            animator.Play("Z_Walk_InPlace");
        }
        else if (!isAttacking)
        {
            StartCoroutine(AttackPlayer());
        }

        float movementSpeed = agent.velocity.magnitude;
        animator.SetFloat("Speed", movementSpeed);
    }

    public void TakeDamage(int damageAmount)
    {
        health -= damageAmount;
        if (health <= 0)
        {
            Die();
        }
    }

    public delegate void ZombieDestroyed(GameObject zombie);
    public event ZombieDestroyed OnZombieDestroyed;

    private void Die()
    {
        animator.Play("Z_Death");
        OnZombieDestroyed?.Invoke(gameObject);
        Destroy(gameObject, 2f);
    }

    private IEnumerator AttackPlayer()
    {
        isAttacking = true;

        while (Vector3.Distance(transform.position, target.position) <= minDistance)
        {
            Debug.Log("Zombie is attacking the player."); // Log attack
            animator.Play("Z_Attack");
            idleMoan.Stop();
            attackSound.Play();
            

            PlayerHealth playerHealth = target.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                Debug.Log("PlayerHealth component found. Applying damage."); // Log damage application
                playerHealth.TakeDamage(damage);
            }
            else
            {
                Debug.LogWarning("PlayerHealth component not found on the target.");
            }

            yield return new WaitForSeconds(attackCooldown); // Wait before the next attack
        }

        isAttacking = false;
        attackSound.Stop();
        idleMoan.Play();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            animator.Play("Z_Attack");
            
        }
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }
}
