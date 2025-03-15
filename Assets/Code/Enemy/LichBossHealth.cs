using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LichBossHealth : MonoBehaviour
{
    public int mainHealth = 20;
    public Slider healthBar;
    public GameObject core;
    public GameObject emblem;
    private Saving savingSystem;
    public bool isShieldActive = true;
    public Image healthBarFill;
    public Color normalColor = Color.red;
    public Color shieldColor = Color.blue;
    public GameObject shieldObject;

    public float shieldCooldown = 10f;
    private bool isShieldOnCooldown = false;

    private void Start()
    {
        if (healthBar != null)
        {
            healthBar.maxValue = mainHealth;
            healthBar.value = mainHealth;
        }

        UpdateHealthBarColor();

        if (shieldObject != null)
        {
            shieldObject.SetActive(isShieldActive);
        }
    }

    public void OnOrbDeactivated()
    {
        if (isShieldActive)
        {
            isShieldActive = false;
            UpdateHealthBarColor();

            if (shieldObject != null)
            {
                shieldObject.SetActive(false);
            }

            // Start the shield cooldown
            if (!isShieldOnCooldown)
            {
                StartCoroutine(ShieldCooldown());
            }
        }
    }

    private System.Collections.IEnumerator ShieldCooldown()
    {
        isShieldOnCooldown = true;
        yield return new WaitForSeconds(shieldCooldown);
        ReactivateShield();
        isShieldOnCooldown = false;
    }

    public void ReactivateShield()
    {
        isShieldActive = true;
        UpdateHealthBarColor();

        if (shieldObject != null)
        {
            shieldObject.SetActive(true);
        }
    }

    private void UpdateHealthBarColor()
    {
        if (healthBarFill != null)
        {
            healthBarFill.color = isShieldActive ? shieldColor : normalColor;
        }
    }

    public void ApplyDamage(int damage)
    {
        if (isShieldActive) return; // No damage if the shield is active

        savingSystem = FindObjectOfType<Saving>();

        if (savingSystem != null)
        {
            // Set the weapon damage to the saved value
            damage = savingSystem.weaponSavedDamage;
        }

        mainHealth -= damage;

        // Update the health bar
        if (healthBar != null)
        {
            healthBar.value = mainHealth;
        }

        // Check if the main health is depleted
        if (mainHealth <= 0)
        {
            DestroyBoss();

            Instantiate(emblem, transform.position, Quaternion.identity);
            DestroyBoss();
        }
    }

    private void DestroyBoss()
    {
       



        Destroy(gameObject);
        Debug.Log("Boss defeated!"); 
        SceneManager.LoadScene(20);
    }

}