using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AOEplayerattack : MonoBehaviour
{
    public int damage = 20; // The amount of damage the weapon deals
    public float maxBlastRadius = 5f; // The maximum radius of the AOE blast
    public float expansionDuration = 1.5f; // Time it takes for the blast to fully expand
    public GameObject fireAOEPrefab; // Reference to the Fire AOE prefab (particle system)
    public Transform playerTransform; // Reference to the player's transform
    public GameObject aoeSphere; // Reference to the sphere GameObject

    private List<EnemyHealth> enemiesHit = new List<EnemyHealth>();
    private bool isAOEActive = false; // Track if the AOE attack is currently active

    public void AOE()
    {
        if (!isAOEActive) // Prevent overlapping AOE attacks
        {
            enemiesHit.Clear();
            StartCoroutine(ExpandingAOEAttack());
        }
    }

    // Coroutine to handle the expanding AOE attack over time
    private IEnumerator ExpandingAOEAttack()
    {
        isAOEActive = true; // Mark AOE as active
        float currentRadius = 0f;
        float timeElapsed = 0f;

        // Activate and reset the AOE sphere
        aoeSphere.SetActive(true);
        aoeSphere.transform.localScale = Vector3.zero;

        while (timeElapsed < expansionDuration)
        {
            // Gradually increase the radius and apply to the sphere's scale
            currentRadius = Mathf.Lerp(0f, maxBlastRadius, timeElapsed / expansionDuration);
            aoeSphere.transform.localScale = Vector3.one * currentRadius * 2; // Adjust for diameter

            DamageEnemiesInRadius(currentRadius);

            timeElapsed += Time.deltaTime;
            yield return null;
        }

        // Ensure the AOE reaches max radius at the end
        DamageEnemiesInRadius(maxBlastRadius);

        // Deactivate the sphere immediately after reaching max radius
        aoeSphere.SetActive(false);

        // Shrink the sphere (hidden, since it's already deactivated)
        yield return StartCoroutine(ShrinkSphere());

        isAOEActive = false; // Mark AOE as inactive
    }

    // Coroutine to shrink the sphere (hidden, since it's already deactivated)
    private IEnumerator ShrinkSphere()
    {
        float shrinkDuration = 0.5f;
        float shrinkTimeElapsed = 0f;
        Vector3 originalScale = aoeSphere.transform.localScale;

        while (shrinkTimeElapsed < shrinkDuration)
        {
            aoeSphere.transform.localScale = Vector3.Lerp(originalScale, Vector3.zero, shrinkTimeElapsed / shrinkDuration);
            shrinkTimeElapsed += Time.deltaTime;
            yield return null;
        }

        // Ensure the sphere is fully shrunk and deactivated
        aoeSphere.transform.localScale = Vector3.zero;
        aoeSphere.SetActive(false);
    }

    // Method to damage enemies within a given radius
    private void DamageEnemiesInRadius(float radius)
    {
        Collider[] hitColliders = Physics.OverlapSphere(playerTransform.position, radius);
        foreach (Collider hit in hitColliders)
        {
            // Skip if the hit object is null
            if (hit == null)
            {
                Debug.LogWarning("Null collider detected in AOE radius.");
                continue;
            }

            // Try to get the EnemyHealth component
            EnemyHealth enemy = hit.GetComponent<EnemyHealth>();
            if (enemy != null)
            {
                if (!enemiesHit.Contains(enemy))
                {
                    try
                    {
                        enemy.TakeDamage(damage);
                        enemiesHit.Add(enemy);
                    }
                    catch (System.Exception e)
                    {
                        Debug.LogError("Error damaging enemy: " + e.Message);
                    }
                }
            }
            else
            {
                Debug.LogWarning("No EnemyHealth component found on: " + hit.name);
            }

            // Try to get the BossSegment component
            BossSegment bossSegment = hit.GetComponent<BossSegment>();
            if (bossSegment != null)
            {
                try
                {
                    bossSegment.TakeDamage(damage); // Deal damage to the boss segment
                }
                catch (System.Exception e)
                {
                    Debug.LogError("Error damaging boss segment: " + e.Message);
                }
            }
        }
    }

    private void PlayFireAOEEffect()
    {
        GameObject fireAOE = Instantiate(fireAOEPrefab, playerTransform.position, Quaternion.identity);
        Destroy(fireAOE, 3f);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, maxBlastRadius);
    }
}