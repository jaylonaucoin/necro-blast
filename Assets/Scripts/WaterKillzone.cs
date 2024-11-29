using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WaterKillZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Check if the player entered the water
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player fell into the water!");

            // Reload the current scene
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
