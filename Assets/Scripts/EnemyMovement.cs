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

    [Header("Sound Effects")]
    public AudioClip[] walkSounds; // Array of ambient walk sounds
    public AudioClip[] attackSounds; // Array of attack sounds
    public AudioClip deathSound;

    private NavMeshAgent agent;
    private Animator animator;
    private CharacterController characterController;
    private AudioSource audioSource;
    private bool isAttacking = false; // To check if the zombie is currently attacking

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();

        // Add an AudioSource component if it doesn't exist
        if (GetComponent<AudioSource>() == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        else
        {
            audioSource = GetComponent<AudioSource>();
        }

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

        // Start playing ambient walk sound
        PlayRandomAmbientSound();
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
        StopAmbientSound();
        PlaySound(deathSound);
        OnZombieDestroyed?.Invoke(gameObject);
        StartCoroutine(FallAndDespawn());
    }

    private IEnumerator FallAndDespawn()
{
    // Rotate the model to make it fall to the ground
    //transform.Rotate(90f, 0f, 0f);

    // Despawn the enemy after 10 seconds
    yield return new WaitForSeconds(0f);
    Destroy(gameObject);
}


    private IEnumerator AttackPlayer()
    {
        isAttacking = true;
        StopAmbientSound();

        while (Vector3.Distance(transform.position, target.position) <= minDistance)
        {
            Debug.Log("Zombie is attacking the player."); // Log attack
            animator.Play("Z_Attack");
            PlayRandomAttackSound();

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
        ResumeAmbientSound();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            animator.Play("Z_Attack");
            StopAmbientSound();
            PlayRandomAttackSound();
        }
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    private void PlaySound(AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.Stop(); // Stop ambient sound if playing
            audioSource.PlayOneShot(clip);
            Invoke(nameof(ResumeAmbientSound), clip.length); // Resume ambient sound after the clip finishes
        }
    }

    private void PlayRandomAttackSound()
    {
        if (attackSounds.Length > 0)
        {
            int randomIndex = Random.Range(0, attackSounds.Length);
            PlaySound(attackSounds[randomIndex]);
        }
    }

    private void StopAmbientSound()
    {
        if (audioSource.isPlaying && audioSource.loop)
        {
            audioSource.Pause();
        }
    }

    private void ResumeAmbientSound()
    {
        if (walkSounds.Length > 0 && !audioSource.isPlaying)
        {
            PlayRandomAmbientSound();
        }
    }

    private void PlayRandomAmbientSound()
    {
        if (walkSounds.Length > 0)
        {
            int randomIndex = Random.Range(0, walkSounds.Length);
            audioSource.clip = walkSounds[randomIndex];
            audioSource.loop = true;
            audioSource.Play();
        }
    }
}
