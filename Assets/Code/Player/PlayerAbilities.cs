using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAbilities : MonoBehaviour
{
    private enum Ability { Shoot, Shield, AoE }
    private Ability selectedAbility;  // Current selected ability

    // Components
    private ShieldSystem shieldSystem;  // Reference to ShieldSystem
    private Shooter shootAbility;   // Reference to Shooting ability
    private AOEplayerattack aoeAbility; // Reference to AoE ability

    // Ability unlock flags
    public bool isShootUnlocked = false; // True if the Forest boss is defeated
    public bool isShieldUnlocked = false; // True if the Ice boss is defeated
    public bool isAoEUnlocked = false; // True if the Fire boss is defeated

    // UI elements
    public GameObject abilityPanel; // Reference to the ability panel GameObject
    public Image abilityIcon; // Reference to the Image component for the ability icon

    // Icon sprites for abilities
    public Sprite shootIcon;
    public Sprite shieldIcon;
    public Sprite aoeIcon;

    // Public cooldown durations (settable in Inspector)
    [Header("Cooldown Durations (in seconds)")]
    public float shootCooldown = 2f; // Cooldown for Shoot
    public float shieldCooldown = 5f; // Cooldown for Shield
    public float aoeCooldown = 10f; // Cooldown for AoE
    public GameObject Abilityhole;

    // Cooldown states
    private bool canUseShoot = true;
    private bool canUseShield = true;
    private bool canUseAoE = true;

    // Remaining cooldown times
    private float shootCooldownRemaining = 0f;
    private float shieldCooldownRemaining = 0f;
    private float aoeCooldownRemaining = 0f;

    Saving save;
    public Slider cooldownSlider;

    private void Start()
    {
        save = GameObject.FindObjectOfType<Saving>();
        // Get references to the components
        shieldSystem = GetComponent<ShieldSystem>();
        shootAbility = GetComponent<Shooter>(); // Assuming you have a ShootAbility script
        aoeAbility = GetComponent<AOEplayerattack>(); // Assuming you have an AoeAbility script

        // Set initial selected ability (optional)
        selectedAbility = Ability.Shoot; // Default to Shoot

        isShootUnlocked = save.isShootUnlocked;
        isShieldUnlocked = save.isShieldUnlocked;
        isAoEUnlocked = save.isAoEUnlocked;

        // Update the ability panel on start
        UpdateAbilityPanel();

        // Set the slider to inactive by default
        if (cooldownSlider != null)
        {
            cooldownSlider.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        // Ability selection
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SelectAbility(Ability.Shoot);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SelectAbility(Ability.Shield);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SelectAbility(Ability.AoE);
        }

        // Use the selected ability
        if (Input.GetKeyDown(KeyCode.Q))
        {
            UseAbility();
        }

        // Update the ability panel every frame
        UpdateAbilityPanel();

        // Update the cooldown slider based on the selected ability
        UpdateCooldownSlider();

        // Unlock abilities if conditions are met
        if (save.isShootUnlocked == true)
        {
            UnlockShootAbility();
        }
        if (save.isAoEUnlocked == true)
        {
            UnlockAoEAbility();
        }
        if (save.isShieldUnlocked == true)
        {
            UnlockShieldAbility();
        }
    }

    private void SelectAbility(Ability ability)
    {
        // Check if the ability is unlocked before allowing selection
        if ((ability == Ability.Shoot && isShootUnlocked) ||
            (ability == Ability.Shield && isShieldUnlocked) ||
            (ability == Ability.AoE && isAoEUnlocked))
        {
            selectedAbility = ability;
            Debug.Log("Selected Ability: " + selectedAbility);
            UpdateAbilityPanel(); // Update UI

            // Show the slider if the selected ability is on cooldown
            UpdateSliderVisibility();
        }
        else
        {
            Debug.Log("This ability is locked!");
        }
    }

    private void UseAbility()
    {
        switch (selectedAbility)
        {
            case Ability.Shoot:
                if (isShootUnlocked && canUseShoot)
                {
                    shootAbility.ShootProjectile(); // Call the shooting method
                    StartCoroutine(AbilityCooldown("Shoot", shootCooldown));
                }
                else if (!isShootUnlocked)
                {
                    Debug.Log("Shoot ability is locked! Defeat the Forest boss.");
                }
                else
                {
                    Debug.Log("Shoot ability is on cooldown!");
                }
                break;

            case Ability.Shield:
                if (isShieldUnlocked && canUseShield)
                {
                    shieldSystem.ActivateShield(); // Activate the shield
                    StartCoroutine(AbilityCooldown("Shield", shieldCooldown));
                }
                else if (!isShieldUnlocked)
                {
                    Debug.Log("Shield ability is locked! Defeat the Ice boss.");
                }
                else
                {
                    Debug.Log("Shield ability is on cooldown!");
                }
                break;

            case Ability.AoE:
                if (isAoEUnlocked && canUseAoE)
                {
                    aoeAbility.AOE(); // Call the AoE method
                    StartCoroutine(AbilityCooldown("AoE", aoeCooldown));
                }
                else if (!isAoEUnlocked)
                {
                    Debug.Log("AoE ability is locked! Defeat the Fire boss.");
                }
                else
                {
                    Debug.Log("AoE ability is on cooldown!");
                }
                break;

            default:
                Debug.LogWarning("No ability selected!");
                break;
        }
    }

    private IEnumerator AbilityCooldown(string abilityName, float cooldown)
    {
        // Show the slider when cooldown starts
        if (cooldownSlider != null)
        {
            cooldownSlider.gameObject.SetActive(true);
        }

        // Disable the ability
        switch (abilityName)
        {
            case "Shoot":
                canUseShoot = false;
                shootCooldownRemaining = cooldown;
                break;

            case "Shield":
                canUseShield = false;
                shieldCooldownRemaining = cooldown;
                break;

            case "AoE":
                canUseAoE = false;
                aoeCooldownRemaining = cooldown;
                break;
        }

        // Gradually decrease the remaining cooldown time
        while (true)
        {
            switch (abilityName)
            {
                case "Shoot":
                    shootCooldownRemaining -= Time.deltaTime;
                    if (shootCooldownRemaining <= 0)
                    {
                        shootCooldownRemaining = 0;
                        canUseShoot = true;
                        Debug.Log("Shoot ability is ready to use again!");
                        UpdateSliderVisibility(); // Hide the slider if the current ability's cooldown is done
                        yield break;
                    }
                    break;

                case "Shield":
                    shieldCooldownRemaining -= Time.deltaTime;
                    if (shieldCooldownRemaining <= 0)
                    {
                        shieldCooldownRemaining = 0;
                        canUseShield = true;
                        Debug.Log("Shield ability is ready to use again!");
                        UpdateSliderVisibility(); // Hide the slider if the current ability's cooldown is done
                        yield break;
                    }
                    break;

                case "AoE":
                    aoeCooldownRemaining -= Time.deltaTime;
                    if (aoeCooldownRemaining <= 0)
                    {
                        aoeCooldownRemaining = 0;
                        canUseAoE = true;
                        Debug.Log("AoE ability is ready to use again!");
                        UpdateSliderVisibility(); // Hide the slider if the current ability's cooldown is done
                        yield break;
                    }
                    break;
            }
            yield return null;
        }
    }

    private void UpdateCooldownSlider()
    {
        // Update the slider based on the selected ability's remaining cooldown
        switch (selectedAbility)
        {
            case Ability.Shoot:
                cooldownSlider.maxValue = shootCooldown;
                cooldownSlider.value = shootCooldownRemaining;
                break;

            case Ability.Shield:
                cooldownSlider.maxValue = shieldCooldown;
                cooldownSlider.value = shieldCooldownRemaining;
                break;

            case Ability.AoE:
                cooldownSlider.maxValue = aoeCooldown;
                cooldownSlider.value = aoeCooldownRemaining;
                break;
        }
    }

    private void UpdateSliderVisibility()
    {
        // Show the slider if the selected ability is on cooldown, otherwise hide it
        switch (selectedAbility)
        {
            case Ability.Shoot:
                cooldownSlider.gameObject.SetActive(shootCooldownRemaining > 0);
                break;

            case Ability.Shield:
                cooldownSlider.gameObject.SetActive(shieldCooldownRemaining > 0);
                break;

            case Ability.AoE:
                cooldownSlider.gameObject.SetActive(aoeCooldownRemaining > 0);
                break;
        }
    }

    // Call these methods when bosses are defeated to unlock abilities
    public void UnlockShootAbility()
    {
        save.isShootUnlocked = true;
        isShootUnlocked = true;
        Debug.Log("Shoot ability unlocked!");
        UpdateAbilityPanel(); // Update the panel when an ability is unlocked
        save.SavePlayerData();
    }

    public void UnlockShieldAbility()
    {
        save.isShieldUnlocked = true;
        isShieldUnlocked = true;
        Debug.Log("Shield ability unlocked!");
        UpdateAbilityPanel(); // Update the panel when an ability is unlocked
        save.SavePlayerData();
    }

    public void UnlockAoEAbility()
    {
        save.isAoEUnlocked = true;
        isAoEUnlocked = true;
        Debug.Log("AoE ability unlocked!");
        UpdateAbilityPanel(); // Update the panel when an ability is unlocked
        save.SavePlayerData();
    }

    private void UpdateAbilityPanel()
    {
        // Check if no abilities are unlocked
        if (!isShootUnlocked && !isShieldUnlocked && !isAoEUnlocked)
        {
            // Disable the ability panel
            if (abilityPanel != null)
            {
                abilityPanel.SetActive(false);
            }
        }
        else
        {
            // Enable the ability panel if at least one ability is unlocked
            if (abilityPanel != null)
            {
                abilityPanel.SetActive(true);
            }

            // Update the ability icon based on the selected ability
            if (abilityIcon != null)
            {
                switch (selectedAbility)
                {
                    case Ability.Shoot:
                        abilityIcon.sprite = shootIcon;
                        break;
                    case Ability.Shield:
                        abilityIcon.sprite = shieldIcon;
                        break;
                    case Ability.AoE:
                        abilityIcon.sprite = aoeIcon;
                        break;
                }
            }
        }
    }
}