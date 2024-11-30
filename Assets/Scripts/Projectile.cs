using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float damageAmount = 10f; // Damage that the projectile deals

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the projectile hit the player
        if (collision.collider.CompareTag("Player"))
        {
            // Get the PlayerHealth component and deal damage
            PlayerHealth playerHealth = collision.collider.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damageAmount);
            }

            // Destroy the projectile
            Destroy(gameObject);
        }
        else
        {
            // Optionally destroy the projectile if it hits something else
            Destroy(gameObject);
        }
    }
}
