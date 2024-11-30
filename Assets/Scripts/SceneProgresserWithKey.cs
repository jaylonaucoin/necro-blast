using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoaderWithKey : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Check if the key has been collected
            if (KeyTracker.hasKey)
            {
                // Load the next scene
                int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;

                if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
                {
                    SceneManager.LoadScene(nextSceneIndex);
                }
                else
                {
                    // Loop back to the first scene
                    SceneManager.LoadScene(0);
                }
            }
            else
            {
                Debug.Log("You need the key to proceed!");
            }
        }
    }
}


