using UnityEngine;
using UnityEngine.UI;

public class HealthUIManager : MonoBehaviour
{
    public PlayerHealth playerHealth; // Reference to the PlayerHealth script
    public Text healthText; // Reference to the UI Text component

    void Update()
    {
        // Update the health display
        if (playerHealth != null && healthText != null)
        {
            healthText.text = $"Health: {playerHealth.CurrentHealth()}";
        }
    }
}
