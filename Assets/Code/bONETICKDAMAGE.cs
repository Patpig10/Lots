using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bONETICKDAMAGE : MonoBehaviour
{
    public int damageAmount = 1;   // Damage per tick
    public float knockbackForce = 5f;  // Optional knockback force
    public float damageInterval = 1f;  // Time between each damage tick

    private HashSet<GameObject> affectedPlayers = new HashSet<GameObject>();

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !affectedPlayers.Contains(other.gameObject))
        {
            affectedPlayers.Add(other.gameObject);
            StartCoroutine(ApplyDamageOverTime(other));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (affectedPlayers.Contains(other.gameObject))
        {
            affectedPlayers.Remove(other.gameObject);
        }
    }

    private IEnumerator ApplyDamageOverTime(Collider player)
    {
        while (affectedPlayers.Contains(player.gameObject))
        {
            ShieldSystem shieldSystem = player.GetComponent<ShieldSystem>();

            if (shieldSystem != null && shieldSystem.shieldActive)
            {
                shieldSystem.TakeDamageWithShield(damageAmount);
            }
            else
            {
                HeartSystem playerHealth = player.GetComponent<HeartSystem>();
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(damageAmount);

                    // Apply knockback (optional)
                    Rigidbody playerRigidbody = player.GetComponent<Rigidbody>();
                    if (playerRigidbody != null)
                    {
                        Vector3 knockbackDirection = (player.transform.position - transform.position).normalized;
                        playerRigidbody.AddForce(knockbackDirection * knockbackForce, ForceMode.Impulse);
                    }
                }
            }

            yield return new WaitForSeconds(damageInterval);
        }
    }
}
