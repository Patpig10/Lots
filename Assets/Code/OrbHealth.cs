using UnityEngine;
using TMPro;

public class OrbHealth : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;
   
    public bool isOrbActive = true;
    private OrbManager orbManager;

    void Start()
    {
        orbManager = FindObjectOfType<OrbManager>();
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damageAmount)
    {
        if (!isOrbActive) return;

        currentHealth -= damageAmount;
        

        if (currentHealth <= 0)
        {
            DeactivateOrb();
        }
    }

 

    private void DeactivateOrb()
    {
        Debug.Log("Orb deactivated!");
        isOrbActive = false;

        LaserController laser = GetComponent<LaserController>();
        if (laser != null)
        {
            laser.DisableLaser();
        }

        GetComponent<Renderer>().material.color = Color.grey;
        if (GetComponent<Collider>() != null)
        {
            GetComponent<Collider>().enabled = false;
        }

        // Notify the OrbManager
        if (orbManager != null)
        {
            orbManager.OnOrbDeactivated(gameObject);
        }
    }

    public void ResetOrb()
    {
        Debug.Log("Orb reset!");
        isOrbActive = true;
        currentHealth = maxHealth;
        GetComponent<Renderer>().material.color = Color.white;

        if (GetComponent<Collider>() != null)
        {
            GetComponent<Collider>().enabled = true;
        }
    }
}
