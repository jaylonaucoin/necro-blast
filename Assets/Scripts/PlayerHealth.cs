using UnityEngine;
using UnityEngine.SceneManagement; // Import this to use scene management functions

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    private float currentHealth;

    void Start()
    {
        currentHealth = maxHealth; // Initialize current health to max health at the start
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        Debug.Log($"Player took {damage} damage. Current health: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(float amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth; // Clamp health to max value
        }
        Debug.Log($"Player healed by {amount}. Current health: {currentHealth}");
    }

    private void Die()
    {
        Debug.Log("Player has died.");
        ReloadScene(); // Reloads the current scene when player dies
    }

    private void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
