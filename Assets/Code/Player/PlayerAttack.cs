using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public Animator animator; // Reference to the Animator component

    void Start()
    {
        animator = GetComponent<Animator>(); // Get the Animator component attached to the GameObject
    }

    void Update()
    {
        // Check if the E key is pressed (Input.GetKeyDown triggers once when pressed)
        if (Input.GetKeyDown(KeyCode.E))
        {
            TriggerAttackAnimation();
        }
    }

    void TriggerAttackAnimation()
    {
        // Set the "Attack" trigger in the Animator
        animator.SetTrigger("Attack");
    }
}
