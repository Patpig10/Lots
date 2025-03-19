using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChecker : MonoBehaviour
{
    public List<GameObject> enemies; // List of enemies to track
    public GameObject fixit; // Object that has the component to enable

    private BlockAndBridgeManager laser; // Reference to the component

    private void Start()
    {
        if (fixit != null)
        {
            laser = fixit.GetComponent<BlockAndBridgeManager>();
        }
    }

    private void Update()
    {
        // Check if all enemies in the list are destroyed
        if (AreAllEnemiesDefeated())
        {
            EnableLaser();
        }
    }

    private bool AreAllEnemiesDefeated()
    {
        // Iterate through the list and check if any enemy is still active
        foreach (GameObject enemy in enemies)
        {
            if (enemy != null) // If an enemy still exists, return false
            {
                return false;
            }
        }
        return true; // All enemies are destroyed
    }

    private void EnableLaser()
    {
        if (laser != null && !laser.enabled)
        {
            laser.enabled = true;
            Debug.Log("All enemies defeated! Laser activated.");
        }
    }
}