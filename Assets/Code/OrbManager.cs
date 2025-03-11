using System.Collections;
using UnityEngine;

public class OrbManager : MonoBehaviour
{
    public GameObject[] orbs; // List of orbs in the scene
    private GameObject activeOrb = null; // The currently active orb
    private LichBossHealth lichBoss; // Reference to the Lich

    private void Start()
    {
        lichBoss = FindObjectOfType<LichBossHealth>();

        if (orbs.Length == 0)
        {
            Debug.LogError("No orbs assigned to OrbManager!");
            return;
        }

        // Deactivate all lasers initially
        DisableAllLaserControllers();

        // Activate a random orb at the start
        ActivateRandomOrb();
    }

    private void DisableAllLaserControllers()
    {
        foreach (GameObject orb in orbs)
        {
            LaserController laser = orb.GetComponent<LaserController>();
            if (laser != null)
            {
                laser.enabled = false; // Completely disable LaserController
            }
        }
    }

    public void OnOrbDeactivated(GameObject deactivatedOrb)
    {
        Debug.Log($"{deactivatedOrb.name} deactivated!");

        // Disable its LaserController
        LaserController laser = deactivatedOrb.GetComponent<LaserController>();
        if (laser != null)
        {
            laser.enabled = false;
        }

        // Notify Lich the shield is down
        lichBoss.OnOrbDeactivated();

        // Start cooldown before selecting a new orb
        StartCoroutine(ShieldCooldown());
    }

    private IEnumerator ShieldCooldown()
    {
        Debug.Log("Shield cooldown started...");

        yield return new WaitForSeconds(lichBoss.shieldCooldown);

        ActivateRandomOrb(); // Select a new orb

        // Reactivate the shield
        lichBoss.ReactivateShield();
    }

    private void ActivateRandomOrb()
    {
        // Find all active orbs
        GameObject[] availableOrbs = System.Array.FindAll(orbs, orb => orb.GetComponent<OrbHealth>().isOrbActive);

        if (availableOrbs.Length == 0)
        {
            Debug.Log("No more orbs available! The Lich is permanently vulnerable.");
            return;
        }

        // Disable all LaserControllers before activating a new one
        DisableAllLaserControllers();

        // Pick a random active orb
        activeOrb = availableOrbs[Random.Range(0, availableOrbs.Length)];

        // Enable LaserController for this orb
        LaserController laser = activeOrb.GetComponent<LaserController>();
        if (laser != null)
        {
            laser.enabled = true;
            laser.SetTargets(activeOrb.transform, lichBoss.core.transform);
            laser.EnableLaser();
        }

        Debug.Log($"Activated laser on {activeOrb.name}.");
    }
}
