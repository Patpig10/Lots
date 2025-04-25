using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractF : MonoBehaviour
{
    public UnityEvent interact;
    private bool canInteract = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered the interaction zone.");
            canInteract = true; // Allow interaction
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player left the interaction zone.");
            canInteract = false; // Disable interaction
        }
    }

    private void Update()
    {
        if (canInteract && Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("Player pressed F to interact.");
            interact.Invoke(); // Invoke assigned UnityEvent
        }

        if (canInteract && Input.GetButtonDown("Talk"))
        {
            Debug.Log("Player pressed F to interact.");
            interact.Invoke(); // Invoke assigned UnityEvent
        }
    }
}
