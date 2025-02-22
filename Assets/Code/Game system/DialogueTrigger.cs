using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DS.ScriptableObjects;
using UnityEngine.UI;

public class DialogueTrigger : MonoBehaviour
{

    [SerializeField] public DSDialogueSO startingDialogue;    // Starting dialogue object
    [SerializeField] public TextMeshProUGUI textUI;           // Text UI for the dialogue text
    [SerializeField] public TextMeshProUGUI nameUI;           // Text UI for the speaker's name
    [SerializeField] public Image Textbox;                    // UI Image for the dialogue textbox

    private DSDialogueSO currentDialogue;                     // Current dialogue being shown
    public bool isPlayerInRange = false;                      // Track if the player is inside the trigger area
    public bool Tutrial = false;
    private void Awake()
    {
        currentDialogue = startingDialogue; // Initialize the starting dialogue
        HideText();                         // Ensure the text is hidden at the start
    }

    private void Update()
    {
        // Check if the player presses the "F" key and is within the trigger area
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.F))
        {
            ShowText(); // Show the dialogue text when "F" is pressed
        }

        if(isPlayerInRange && Tutrial == true)
        {
            ShowText();
        }
    }

    public void ShowText()
    {
        if (currentDialogue != null)
        {
            textUI.text = currentDialogue.Text;        // Display the dialogue text
            nameUI.text = currentDialogue.Speaker;     // Display the speaker's name
            textUI.gameObject.SetActive(true);         // Enable the text UI
            nameUI.gameObject.SetActive(true);         // Enable the name UI
            Textbox.gameObject.SetActive(true);        // Enable the textbox UI
        }
    }

    public void HideText()
    {
        textUI.gameObject.SetActive(false);            // Hide the text UI
        nameUI.gameObject.SetActive(false);            // Hide the name UI
        Textbox.gameObject.SetActive(false);           // Hide the textbox UI
    }

    public void OnOptionChosen(int choiceIndex)
    {
        if (choiceIndex < 0 || choiceIndex >= currentDialogue.Choices.Count)
        {
            Debug.LogError("Invalid choice index");
            return;
        }

        DSDialogueSO nextDialogue = currentDialogue.Choices[choiceIndex].NextDialogue;

        if (nextDialogue == null)
        {
            Debug.Log("End of dialogue");
            HideText(); // Hide the dialogue when there are no more dialogues
            return;
        }

        currentDialogue = nextDialogue;
        ShowText(); // Show the next dialogue text
    }

    // Unity method called when another object enters the trigger collider
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Check if the object is tagged as "Player"
        {
            isPlayerInRange = true;     // Set the flag to true when the player enters
        }
    }

    // Unity method called when another object exits the trigger collider
    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) // Check if the object is tagged as "Player"
        {
            isPlayerInRange = false;    // Set the flag to false when the player exits
            currentDialogue = startingDialogue; // Reset the dialogue to the starting point
            HideText();                 // Hide the dialogue text when the player leaves the area
        }
    }
    public void Reset()
    {
        currentDialogue = startingDialogue; // Reset the dialogue to the starting point
        HideText();                         // Hide the dialogue text
    }
}
