using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyTracker : MonoBehaviour
{
    public static bool hasKey = false; // Static variable to track key status

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            hasKey = true;
            Debug.Log("Key collected!");
            Destroy(gameObject); // Destroy the key object after it's collected
        }
    }
}

