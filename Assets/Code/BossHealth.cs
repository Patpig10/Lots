using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealth : MonoBehaviour
{

    public int mainHealth = 20; // Set the total health of the boss
    public GameObject brain;
    public void ApplyDamage(int damage)
    {
        mainHealth -= damage;

        // Check if the main health is depleted
        if (mainHealth <= 0)
        {
            DestroyBoss();
        }
    }

    private void DestroyBoss()
    {
        // Handle boss destruction (e.g., animations, loot drops, etc.)
        Destroy(gameObject);
        Debug.Log("Boss defeated!");
        Destroy(brain);
    }

}
