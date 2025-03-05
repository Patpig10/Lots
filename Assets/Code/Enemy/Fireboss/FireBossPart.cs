using UnityEngine;

public class FireBossPart : MonoBehaviour
{
    public int partHealth = 1200; // Health of this specific part
    public FireBossHealth bossHealth; // Reference to the main boss health script
    public bool isArm = false; // Whether this part is an arm (can be disabled)

    public void TakeDamage(int damage)
    {
        if (partHealth <= 0) return; // Ignore damage if the part is already disabled

        partHealth -= damage;

        // Apply damage to the main boss health
        if (bossHealth != null)
        {
            bossHealth.TakeDamage(damage);
        }

        // If this is an arm and its health is depleted, disable it
        if (isArm && partHealth <= 0)
        {
            DisablePart();
        }
    }

    private void DisablePart()
    {
        // Disable the arm (e.g., make it invisible or non-functional)
        gameObject.SetActive(false);
        Debug.Log($"{gameObject.name} has been disabled!");
    }
}