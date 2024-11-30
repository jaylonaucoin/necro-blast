using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
public class FlyingEnemyMovement : MonoBehaviour
{
    public float orbitRadius = 3.5f;
    public float orbitSpeed = 3f;
    public float heightOffset = 2f;
    public float approachSpeed = 0.5f;
    public Transform target;
    public int damage = 1;
    public int health = 10;

    public float timeBetweenAttacks = 2f;
    public float attackRange = 10f;
    public float sightRange = 15f;
    public GameObject projectile;

    private NavMeshAgent agent;
    private Animator animator;
    private float angle = 0f;

    private bool alreadyAttacked;
    private bool playerInSightRange;
    private bool playerInAttackRange;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        agent.baseOffset = heightOffset;

        animator.Play("Z_Idle");

        // If there's no target set, assume the player
        if (target == null)
        {
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                target = player.transform;
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

        // Check for sight and attack range
        playerInSightRange = Vector3.Distance(transform.position, target.position) <= sightRange;
        playerInAttackRange = Vector3.Distance(transform.position, target.position) <= attackRange;

        if (playerInSightRange)
        {
            if (playerInAttackRange)
            {
                AttackPlayer();
            }
            else
            {
                OrbitPlayer();
            }
        }
        else
        {
            // Move closer to the target if outside sight range
            agent.SetDestination(target.position);
            animator.Play("Z_Fly");
        }

        // Face the player
        Vector3 lookDirection = target.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(lookDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * orbitSpeed);
    }

    private void OrbitPlayer()
    {
        // Increment the angle for circular motion
        angle += orbitSpeed * Time.deltaTime;

        // Calculate the position in orbit
        float x = target.position.x + orbitRadius * Mathf.Cos(angle * Mathf.Deg2Rad);
        float z = target.position.z + orbitRadius * Mathf.Sin(angle * Mathf.Deg2Rad);
        float y = target.position.y + heightOffset;

        // Use NavMeshAgent to move to the orbit position
        Vector3 orbitPosition = new Vector3(x, y, z);
        agent.SetDestination(orbitPosition);
    }

    private void AttackPlayer()
    {
        if (!alreadyAttacked)
        {
            // Attack code
            Rigidbody rb = Instantiate(projectile, transform.position + transform.forward * 1.5f, Quaternion.identity).GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * 32f, ForceMode.Impulse);
            rb.AddForce(transform.up * 8f, ForceMode.Impulse);

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    public void TakeDamage(int damageAmount)
    {
        health -= damageAmount;
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        animator.Play("Z_Death"); // Play death animation
        Destroy(gameObject, 2f); // Destroy enemy after 2 seconds
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
