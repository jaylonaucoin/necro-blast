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

    private NavMeshAgent agent;
    private Animator animator;
    private CharacterController characterController;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        animator.Play("Z_Idle");

        //sets the speed of the NavMeshAgent
        agent.speed = speed;

        //sets the stopping distance of the NavMesh
        agent.stoppingDistance = minDistance;

        //if theres no target set, assume it is the player
        if (target == null)
        {
            if (GameObject.FindWithTag("Player") != null)
            {
                target = GameObject.FindWithTag("Player").GetComponent<Transform>();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null)
        {
            return;
        }

        //grabs the distance between the enemy and the target
        float distance = Vector3.Distance(transform.position, target.position);

        //moves towards the target as long as its greater than minimum distance
        if (distance > minDistance)
        {
            agent.SetDestination(target.position);
            animator.Play("Z_Walk_InPlace");
        }

        //grabs the current movementSpeed of the enemy
        float movementSpeed = agent.velocity.magnitude;

        //updates the animator with the current movement speed
        animator.SetFloat("Speed", movementSpeed);
    }

    // Method to handle damage
    public void TakeDamage(int damageAmount)
    {
        health -= damageAmount;
        if (health <= 0)
        {
            Die();
        }
    }

    // At the top of the script, add a delegate for the death event
    public delegate void ZombieDestroyed(GameObject zombie);
    public event ZombieDestroyed OnZombieDestroyed;

    private void Die()
    {
        animator.Play("Z_Death"); // Play death animation

        // Notify listeners (e.g., the spawner) that this zombie is destroyed
        if (OnZombieDestroyed != null)
        {
            OnZombieDestroyed(gameObject);
        }

        Destroy(gameObject, 2f); // Destroy enemy after 2 seconds to allow the animation to play
    }


    //check if the enemy enters the target's collider
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            animator.Play("Z_Attack");
            //calls method to take damage
            // PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            // if (playerHealth != null) {
            //     playerHealth.takeDamage(damage);
            // }
        }
    }

    // Set the target of the enemy
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }
}