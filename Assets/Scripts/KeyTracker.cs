/**************************************************************************
 * Filename: KeyTracker.cs
 * Author: Amir Tarbiyat
 * Description:
 *     This script tracks whether the player has collected a key. When the
 *     player interacts with the key object (via a trigger), the key's status
 *     is updated using a static variable, and the key object is destroyed.
 *     Useful for gameplay mechanics that depend on key possession.
 * 
 **************************************************************************/

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

