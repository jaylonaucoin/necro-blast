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

    private NavMeshAgent agent;
    private Animator animator;
    private float angle = 0f; 

    bool alreadyAttacked;
    public GameObject projectile;

    // Start is called before the first frame update
    void Start()
    {
        
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        agent.baseOffset = heightOffset;

        animator.Play("Z_Idle");

        //if theres no target set, assume the player
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

        //calculates distance to target
        float distance = Vector3.Distance(transform.position, target.position);

        //moves to target if outside range
        if (distance > orbitRadius + 1f)
        {
            agent.SetDestination(target.position);
            animator.Play("Z_Fly");
        }
        else
        {
            OrbitPlayer();
        }

        //face the player
        Vector3 lookDirection = target.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(lookDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * orbitSpeed);

        AttackPlayer();
    }

    //orbits around the player
    private void OrbitPlayer()
    {
        //animator.Play("");

        //increment the angle for circular motion
        angle += orbitSpeed * Time.deltaTime;

        //calculate the position in orbit
        float x = target.position.x + orbitRadius * Mathf.Cos(angle * Mathf.Deg2Rad);
        float z = target.position.z + orbitRadius * Mathf.Sin(angle * Mathf.Deg2Rad);
        float y = target.position.y + heightOffset;

        //use NavMeshAgent to move to the orbit position
        Vector3 orbitPosition = new Vector3(x, y, z);
        agent.SetDestination(orbitPosition);
    }

     private void AttackPlayer()
    {

        if (!alreadyAttacked)
        {
            ///Attack code here
            Rigidbody rb = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * 32f, ForceMode.Impulse);
            rb.AddForce(transform.up * 8f, ForceMode.Impulse);
            ///End of attack code

        }
    }

    //taking damage
    public void TakeDamage(int damageAmount)
    {
        health -= damageAmount;
        if (health <= 0)
        {
            Die();
        }
    }

    // Handle enemy death
    private void Die()
    {
        animator.Play("Z_Death"); // Play death animation
        Destroy(gameObject, 2f); // Destroy enemy after 2 seconds
    }

    //check if the enemy enters the target's collider
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            animator.Play("Z_Attack");
            //calls method to take damage
            // PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            // if (playerHealth != null)
            // {
            //     playerHealth.TakeDamage(damage);
            // }
        }
    }

    // Set a new target for the enemy
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }
}
