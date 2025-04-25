using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class QuestStart : MonoBehaviour
{
    public GameObject questStartUI; // Reference to the quest start UI GameObject

    void Start()
    {
        // Ensure UI is hidden at the start
        if (questStartUI != null)
            questStartUI.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        // Check if the player entered the trigger area
        if (other.CompareTag("Player"))
        {
           // questStartUI.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        // Check if the player left the trigger area
        if (other.CompareTag("Player"))
        {
            questStartUI.SetActive(false);
        }
    }
}
