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
    public Image abilityIcon;

    // Icon sprites for abilities
    public Sprite shootIcon;
    public Sprite shieldIcon;
    public Sprite aoeIcon;

    // Public cooldown durations (settable in Inspector)
    [Header("Cooldown Durations (in seconds)")]
    public float shootCooldown = 2f; // Cooldown for Shoot
    public float shieldCooldown = 5f; // Cooldown for Shield
    public float aoeCooldown = 10f; // Cooldown for AoE

    // Cooldown states
    private bool canUseShoot = true;
    private bool canUseShield = true;
    private bool canUseAoE = true;

    private void Start()
    {
        // Get references to the components
        shieldSystem = GetComponent<ShieldSystem>();
        shootAbility = GetComponent<Shooter>(); // Assuming you have a ShootAbility script
        aoeAbility = GetComponent<AOEplayerattack>(); // Assuming you have an AoeAbility script

        // Set initial selected ability (optional)
        selectedAbility = Ability.Shoot; // Default to Shoot
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
        UpdateAbilityIcon();
    }

    private void SelectAbility(Ability ability)
    {
        selectedAbility = ability;
        Debug.Log("Selected Ability: " + selectedAbility);
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
        switch (abilityName)
        {
            case "Shoot":
                canUseShoot = false;
                break;

            case "Shield":
                canUseShield = false;
                break;

            case "AoE":
                canUseAoE = false;
                break;
        }

        yield return new WaitForSeconds(cooldown);

        switch (abilityName)
        {
            case "Shoot":
                canUseShoot = true;
                break;

            case "Shield":
                canUseShield = true;
                break;

            case "AoE":
                canUseAoE = true;
                break;
        }

        Debug.Log(abilityName + " is ready to use again!");
    }

    // Call these methods when bosses are defeated to unlock abilities
    public void UnlockShootAbility()
    {
        isShootUnlocked = true;
        Debug.Log("Shoot ability unlocked!");
    }

    public void UnlockShieldAbility()
    {
        isShieldUnlocked = true;
        Debug.Log("Shield ability unlocked!");
    }

    public void UnlockAoEAbility()
    {
        isAoEUnlocked = true;
        Debug.Log("AoE ability unlocked!");
    }

    private void UpdateAbilityIcon()
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
