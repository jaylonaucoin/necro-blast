/**************************************************************************
 * Filename: PlayerHealth.cs
 * Author: Amir Tarbiyat
 * Description:
 *     This script manages the player's health, including taking damage,
 *     healing, dying, and updating the health display on the screen.
 * 
 **************************************************************************/


using UnityEngine;
using UnityEngine.SceneManagement; // Import this to use scene management functions

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    private float currentHealth;

    public TextMesh healthTextMesh; // Reference to the legacy Text Mesh

    void Start()
    {
        currentHealth = maxHealth; // Initialize current health to max health at the start
        UpdateHealthText(); // Update the text at the start
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        Debug.Log($"Player took {damage} damage. Current health: {currentHealth}");
        UpdateHealthText(); // Update the text

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(float amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Min(currentHealth, maxHealth); // Clamp health to max value
        Debug.Log($"Player healed by {amount}. Current health: {currentHealth}");
        UpdateHealthText(); // Update the text
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

    private void UpdateHealthText()
    {
        if (healthTextMesh != null)
        {
            healthTextMesh.text = $"{currentHealth}/{maxHealth}";
        }
    }
}
