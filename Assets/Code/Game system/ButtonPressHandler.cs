using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;

public class ButtonPressHandler : MonoBehaviour
{
    public Button targetButton; // Main button to track presses
    public GameObject[] buttonsToDelete; // Buttons to delete permanently
    public GameObject[] toggleButtons; // Buttons that can be toggled on/off
    public bool land3;
    private static int pressCount = 0; // Tracks button presses
    private static bool buttonsDeleted = false; // Prevents buttons from coming back
    public Saving Saving;
    public int count = 9; // Number of presses needed for deletion
    private string saveFilePath;

    void Awake()
    {
        saveFilePath = Application.persistentDataPath + "/buttonSave.json";
        LoadButtonState();
    }

    void Start()
    {
        if (buttonsDeleted)
        {
            DeleteButtons();
        }
        else if (targetButton != null)
        {
            targetButton.onClick.AddListener(HandleButtonPress);
        }
    }

    private void HandleButtonPress()
    {
        pressCount++;

        if (pressCount >= count)
        {
            buttonsDeleted = true;
            DeleteButtons();
            SaveButtonState();
        }
    }

    private void DeleteButtons()
    {
        foreach (GameObject button in buttonsToDelete)
        {
            if (button != null)
            {
                Saving.Addlevel3();
                Destroy(button);
            }
        }
        Debug.Log("All buttons have been permanently deleted.");
    }

    private void SaveButtonState()
    {
        ButtonSaveData data = new ButtonSaveData { pressCount = pressCount, buttonsDeleted = buttonsDeleted };
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(saveFilePath, json);
        Debug.Log("Button state saved.");
    }

    private void LoadButtonState()
    {
        if (File.Exists(saveFilePath))
        {
            string json = File.ReadAllText(saveFilePath);
            ButtonSaveData data = JsonUtility.FromJson<ButtonSaveData>(json);
            pressCount = data.pressCount;
            buttonsDeleted = data.buttonsDeleted;
            Debug.Log("Button state loaded.");
        }
    }

    public void ResetButtonPress()
    {
        pressCount = 0;
        buttonsDeleted = false;
        SaveButtonState();
        Debug.Log("Button press count reset. Buttons will reappear.");
    }
}
[Serializable]
public class ButtonSaveData
{
    public int pressCount;
    public bool buttonsDeleted;
}
