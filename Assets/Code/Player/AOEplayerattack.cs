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

    private void Update()
    {
        // Check if the player presses the 'T' key
        if (Input.GetKeyDown(KeyCode.T))
        {
            enemiesHit.Clear(); // Clear the list of previously hit enemies
            StartCoroutine(ExpandingAOEAttack()); // Start the expanding AOE attack
            PlayFireAOEEffect(); // Play the fire AOE effect
        }
    }

    public void AOE()
    {
        enemiesHit.Clear();
        StartCoroutine(ExpandingAOEAttack());
      //  PlayFireAOEEffect();
    }

    // Coroutine to handle the expanding AOE attack over time
    public IEnumerator ExpandingAOEAttack()
    {
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

        // Ensure the AOE reaches max radius at the end, then start shrinking
        DamageEnemiesInRadius(maxBlastRadius);
        aoeSphere.SetActive(false); // Deactivate sphere once it's fully expanded
        yield return StartCoroutine(ShrinkSphere());
    }

    // Coroutine to shrink the sphere and deactivate it
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

        aoeSphere.SetActive(false); // Deactivate sphere once it's shrunk
    }

    // Method to damage enemies within a given radius
    private void DamageEnemiesInRadius(float radius)
    {
        Collider[] hitColliders = Physics.OverlapSphere(playerTransform.position, radius);
        foreach (Collider hit in hitColliders)
        {
            EnemyHealth enemy = hit.GetComponent<EnemyHealth>();
            if (enemy != null && !enemiesHit.Contains(enemy))
            {
                enemy.TakeDamage(damage);
                enemiesHit.Add(enemy);
            }
            BossSegment bossSegment = hit.GetComponent<BossSegment>();
            if (bossSegment != null)
            {
                bossSegment.TakeDamage(damage); // Deal damage to the boss segment
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
