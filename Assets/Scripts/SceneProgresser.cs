using UnityEngine;
using UnityEngine.SceneManagement; // Required to manage scenes

public class SceneLoader : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Check if the player entered the trigger
        if (other.CompareTag("Player"))
        {
            // Load the next scene by build index
            int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;

            // Ensure the next scene index is valid
            if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
            {
                SceneManager.LoadScene(nextSceneIndex);
            }
            else
            {
                Debug.Log("No more scenes to load. This is the last level.");
            }
        }
    }
}
