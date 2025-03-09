using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoryButtonManager : MonoBehaviour
{
    public GameObject storyButton2; // GameObject for condition 1
    public GameObject storyButton3; // GameObject for condition 2
    public GameObject generalButton; // GameObject for general condition

    public Saving saving; // Reference to the Saving class

    void Start()
    {
        // Find the Saving component in the scene
        saving = FindObjectOfType<Saving>();

        if (saving == null)
        {
            Debug.LogError("Saving component not found in the scene!");
            return;
        }

        // Check conditions and activate buttons
        CheckButtons();
    }

    public void Update()
    {
        // Check conditions and activate buttons
        CheckButtons();
        LoadButtonStates();
    }

    void LoadButtonStates()
    {
        // Load the saved state for each button
        if (saving.storyButton2Destroyed)
        {
            storyButton2.SetActive(false); // Keep the button inactive if it was destroyed
        }

        if (saving.storyButton3Destroyed)
        {
            storyButton3.SetActive(false); // Keep the button inactive if it was destroyed
        }

        if (saving.generalButtonDestroyed)
        {
            generalButton.SetActive(false); // Keep the button inactive if it was destroyed
        }
    }

    void CheckButtons()
    {
        // Condition 1: levelUnlocked == 5 && Grassemblem == true
        if (saving.levelUnlocked == 5 && saving.Grassemblem)
        {
            storyButton2.SetActive(true); // Ensure the GameObject is active
            storyButton2.GetComponent<Button>().onClick.AddListener(() => OnButtonPressed(storyButton2));
        }
        else
        {
            storyButton2.SetActive(false); // Hide the GameObject if the condition is not met
        }

        // Condition 2: Iceemblem == true && Fireemblem == true
        if (saving.Iceemblem && saving.Fireemblem)
        {
            storyButton3.SetActive(true); // Ensure the GameObject is active
            storyButton3.GetComponent<Button>().onClick.AddListener(() => OnButtonPressed(storyButton3));
        }
        else
        {
            storyButton3.SetActive(false); // Hide the GameObject if the condition is not met
        }

        // General Condition: levelUnlocked >= 2
        if (saving.levelUnlocked >= 2)
        {
            generalButton.SetActive(true); // Ensure the GameObject is active
            generalButton.GetComponent<Button>().onClick.AddListener(() => OnButtonPressed(generalButton));
        }
        else
        {
            generalButton.SetActive(false); // Hide the GameObject if the condition is not met
        }
    }

    void OnButtonPressed(GameObject buttonObject)
    {
        // Destroy the entire GameObject when the button is pressed
        Destroy(buttonObject);

        // Save the game state (optional, if you want to persist the button state)
        saving.SavePlayerData();

    }

    public void OnDestroy1()
    {
        saving.generalButtonDestroyed = true;
        saving.SavePlayerData();
    }

    public void OnDestroy2()
    {
        saving.storyButton2Destroyed = true;
        saving.SavePlayerData();
    }

    public void OnDestroy3()
    {
        saving.storyButton3Destroyed = true;
        saving.SavePlayerData();
    }
}

