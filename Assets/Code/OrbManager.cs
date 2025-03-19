using System.Collections;
using UnityEngine;

public class OrbManager : MonoBehaviour
{
    public GameObject[] orbs; // List of orbs
    public int activeOrbCount = 6; // Ensure this matches the number of orbs
    private LichBossHealth lichBoss;
    public GameObject text;
    public GameObject bomb;
    public GameObject aoeSphere; // Reference to the AOE sphere
    public float expansionDuration = 2f; // Duration for the AOE expansion
    public float maxBlastRadius = 10f; // Maximum radius of the AOE
    private bool isAOEActive = false; // Track if AOE is active

    private void Start()
    {
        lichBoss = FindObjectOfType<LichBossHealth>();

        if (orbs.Length == 0)
        {
            Debug.LogError("No orbs assigned to OrbManager!");
            return;
        }

        ResetOrbs(); // Initialize orbs
        ActivateNextOrb(); // Start with the first orb
    }

    public IEnumerator Blast()
    {
        Debug.Log("Blast started!"); // Debug

        bomb.transform.localScale += new Vector3(1, 1, 1);
        text.SetActive(true);

        // Instantiate the AOE sphere dynamically
        GameObject newAoeSphere = Instantiate(aoeSphere, transform.position, Quaternion.identity);
        newAoeSphere.SetActive(true);

        isAOEActive = true;
        float currentRadius = 0f;
        float timeElapsed = 0f;

        // Start with zero scale
        newAoeSphere.transform.localScale = Vector3.zero;

        while (timeElapsed < expansionDuration)
        {
            currentRadius = Mathf.Lerp(0f, maxBlastRadius, timeElapsed / expansionDuration);
            newAoeSphere.transform.localScale = Vector3.one * (currentRadius * 2); // Diameter scaling

           // DamageEnemiesInRadius(currentRadius, newAoeSphere.transform.position);

            timeElapsed += Time.deltaTime;
            yield return null;
        }

        Debug.Log("Blast max size reached!"); // Debug

        // Ensure final damage
        //DamageEnemiesInRadius(maxBlastRadius, newAoeSphere.transform.position);

        yield return new WaitForSeconds(0.5f); // Let the effect be visible before disabling

        Destroy(newAoeSphere); // Destroy the AOE sphere after the effect ends
        Debug.Log("AOE blast ended!"); // Debug

        isAOEActive = false;
    }
    private IEnumerator ShrinkSphere()
    {
        // Implement the shrinking logic here
        yield return null;
    }

    private void DamageEnemiesInRadius(float radius)
    {
        // Implement logic to damage enemies within the given radius
    }

    private void ResetOrbs()
    {
        //activeOrbCount = orbs.Length; // Reset orb count

        foreach (GameObject orb in orbs)
        {
            OrbHealth orbHealth = orb.GetComponent<OrbHealth>();
            LaserController laser = orb.GetComponent<LaserController>();

            if (orbHealth != null)
            {
                orbHealth.isOrbActive = true; // Reset orb state
            }

            if (laser != null)
            {
                laser.enabled = false; // Keep all lasers off initially
            }
        }

        Debug.Log("All orbs have been reset.");
    }

    public void OnOrbDeactivated(GameObject deactivatedOrb)
    {
        Debug.Log($"{deactivatedOrb.name} deactivated!");

        OrbHealth orbHealth = deactivatedOrb.GetComponent<OrbHealth>();
        LaserController laser = deactivatedOrb.GetComponent<LaserController>();

        if (orbHealth != null)
        {
            orbHealth.isOrbActive = false;
        }

        if (laser != null)
        {
            laser.enabled = false;
        }

        activeOrbCount--;

        if (lichBoss != null)
        {
            lichBoss.OnOrbDeactivated(); // Disable shield when an orb is hit
        }

        // Check if all orbs are deactivated
        if (activeOrbCount <= 0)
        {
            Debug.Log("All orbs deactivated. Resetting orbs...");
            StartCoroutine(Blast()); // Start the AOE effect
           // ResetOrbs(); // Reset orbs after the blast
            ActivateNextOrb(); // Activate the next orb immediately after reset
        }
        else
        {
            StartCoroutine(ShieldCooldown());
        }
    }

    private IEnumerator ShieldCooldown()
    {
        Debug.Log("Shield cooldown started...");
        yield return new WaitForSeconds(lichBoss.shieldCooldown);

        ActivateNextOrb(); // Try to activate the next available orb

        if (HasActiveOrb() && lichBoss != null)
        {
            lichBoss.ReactivateShield();
        }
        else
        {
            Debug.Log("No active orbs detected. Shield remains down.");
        }
    }

    private bool HasActiveOrb()
    {
        foreach (GameObject orb in orbs)
        {
            OrbHealth orbHealth = orb.GetComponent<OrbHealth>();
            if (orbHealth != null && orbHealth.isOrbActive)
            {
                return true;
            }
        }
        return false;
    }

    public void ActivateNextOrb()
    {
        if (lichBoss == null || !lichBoss.isShieldActive) return; // Only activate a laser if the shield is up

        foreach (GameObject orb in orbs)
        {
            OrbHealth orbHealth = orb.GetComponent<OrbHealth>();
            LaserController laser = orb.GetComponent<LaserController>();

            if (orbHealth != null && orbHealth.isOrbActive && laser != null)
            {
                laser.enabled = true; // Enable the laser only when shield is active
                Debug.Log($"Activated laser on {orb.name}.");
                return;
            }
        }

        Debug.LogWarning("No valid orbs found to activate!");
    }
}