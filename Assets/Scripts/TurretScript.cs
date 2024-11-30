using UnityEngine;

public class TurretEnemy : MonoBehaviour
{
    public Transform player;

    public LayerMask whatIsPlayer;

    public float health;

    // Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    public GameObject projectile;

    // Ranges
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    public float rotationSpeed = 5f; // Speed for rotating towards the player

    private void Awake()
    {
        player = GameObject.Find("Player").transform;
    }

    private void Update()
    {
        // Check for sight and attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (playerInSightRange)
        {
            RotateTowardsPlayer();
        }

        if (playerInAttackRange && playerInSightRange)
        {
            AttackPlayer();
        }
    }

    private void RotateTowardsPlayer()
    {
        // Calculate the direction to the player
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0; // Prevent the turret from tilting up or down

        // Smoothly rotate towards the player
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
    }

    private void AttackPlayer()
    {
        if (!alreadyAttacked)
        {
            // Calculate direction to the player for projectile
            Vector3 directionToPlayer = (player.position - transform.position).normalized;

            // Attack code here
            Rigidbody rb = Instantiate(projectile, transform.position + transform.forward * 1.5f + Vector3.up * 1.0f, Quaternion.identity).GetComponent<Rigidbody>();

            // Apply force directly towards the player's position with some arc adjustment
            Vector3 forceDirection = (player.position - rb.transform.position).normalized;
            rb.AddForce(forceDirection * 32f, ForceMode.Impulse);
            // End of attack code

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0) Invoke(nameof(DestroyEnemy), 0.5f);
    }

    private void DestroyEnemy()
    {
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
