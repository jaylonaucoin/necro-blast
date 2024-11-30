using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    public GameObject zombiePrefab; // The zombie prefab
    public Transform spawnPoint;   // Where zombies will spawn
    public float spawnInterval = 7f; // Time between spawns
    public int maxZombies = 10;    // Maximum number of zombies allowed

    private List<GameObject> activeZombies = new List<GameObject>(); // List to track zombies

    void Start()
    {
        // Start the spawn cycle
        StartCoroutine(SpawnZombies());
    }

    IEnumerator SpawnZombies()
    {
        while (true)
        {
            // Wait for the spawn interval
            yield return new WaitForSeconds(spawnInterval);

            // Check if there's room to spawn a new zombie
            if (activeZombies.Count < maxZombies)
            {
                GameObject newZombie = Instantiate(zombiePrefab, spawnPoint.position, spawnPoint.rotation);
                activeZombies.Add(newZombie);

                // Subscribe to the zombie's destruction event
                EnemyMovement zombieScript = newZombie.GetComponent<EnemyMovement>();
                if (zombieScript != null)
                {
                    zombieScript.OnZombieDestroyed += RemoveZombieFromList;
                }
            }
        }
    }

    void RemoveZombieFromList(GameObject zombie)
    {
        if (activeZombies.Contains(zombie))
        {
            activeZombies.Remove(zombie);
        }
    }
}
