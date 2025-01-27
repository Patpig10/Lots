using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Battledoor : MonoBehaviour
{

    // Range within which enemies are checked
    public float range = 10f;

    // Tag for enemies
    public string enemyTag = "Enemy";
    public GameObject path;
    // Update is called once per frame
    void Update()
    {
        CheckEnemiesAndDestroy();
    }

    // Check if all enemies in range are destroyed
    void CheckEnemiesAndDestroy()
    {
        // Find all objects with the "Enemy" tag
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);

        // Check if any enemy is within range
        bool enemyInRange = false;

        foreach (GameObject enemy in enemies)
        {
            if (Vector3.Distance(transform.position, enemy.transform.position) <= range)
            {
                enemyInRange = true;
                break; // Exit loop if at least one enemy is found
            }
        }

        // If no enemies are in range, destroy this GameObject
        if (!enemyInRange)
        {
            path.SetActive(true);
            Destroy(gameObject);
        }
    }

    // Draw the range in the Scene view for debugging
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
