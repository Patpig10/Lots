using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilities : MonoBehaviour
{
    private enum Ability { Shoot, Shield, AoE }
    private Ability selectedAbility;  // Current selected ability

    // Components
    private ShieldSystem shieldSystem;  // Reference to ShieldSystem
    private Shooter shootAbility;   // Reference to Shooting ability
    private AOEplayerattack aoeAbility;        // Reference to AoE ability

    // Ability unlock flags
    public bool isShootUnlocked = false; // True if the Forest boss is defeated
    public bool isShieldUnlocked = false; // True if the Ice boss is defeated
    public bool isAoEUnlocked = false; // True if the Fire boss is defeated

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
                if (isShootUnlocked)
                {
                    shootAbility.ShootProjectile(); // Call the shooting method
                }
                else
                {
                    Debug.Log("Shoot ability is locked! Defeat the Forest boss.");
                }
                break;

            case Ability.Shield:
                if (isShieldUnlocked)
                {
                    shieldSystem.ActivateShield(); // Activate the shield
                }
                else
                {
                    Debug.Log("Shield ability is locked! Defeat the Ice boss.");
                }
                break;

            case Ability.AoE:
                if (isAoEUnlocked)
                {
                    aoeAbility.AOE(); // Call the AoE method
                }
                else
                {
                    Debug.Log("AoE ability is locked! Defeat the Fire boss.");
                }
                break;

            default:
                Debug.LogWarning("No ability selected!");
                break;
        }
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
}
