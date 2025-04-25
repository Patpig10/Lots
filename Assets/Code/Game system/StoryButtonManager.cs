using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoryButtonManager : MonoBehaviour
{
    public GameObject storyButton2;
    public GameObject storyButton3;
    public GameObject generalButton;

    public Saving saving;

    void Start()
    {
        saving = FindObjectOfType<Saving>();

        if (saving == null)
        {
            Debug.LogError("Saving component not found in the scene!");
            return;
        }

        LoadButtonStates();
        CheckButtons();
    }

    void LoadButtonStates()
    {
        if (saving.storyButton2Destroyed)
        {
            storyButton2.SetActive(false);
        }

        if (saving.storyButton3Destroyed)
        {
            storyButton3.SetActive(false);
        }

        if (saving.generalButtonDestroyed)
        {
            generalButton.SetActive(false);
        }
    }

    void CheckButtons()
    {
        if (saving.levelUnlocked == 5 && saving.Grassemblem && !saving.storyButton2Destroyed)
        {
            ActivateButton(storyButton2, () => OnButtonPressed(storyButton2, 2));
        }
        else
        {
            storyButton2.SetActive(false);
        }

        if (saving.Iceemblem && saving.Fireemblem && !saving.storyButton3Destroyed)
        {
            ActivateButton(storyButton3, () => OnButtonPressed(storyButton3, 3));
        }
        else
        {
            storyButton3.SetActive(false);
        }

        if (saving.levelUnlocked >= 2 && !saving.generalButtonDestroyed)
        {
            ActivateButton(generalButton, () => OnButtonPressed(generalButton, 1));
        }
        else
        {
            generalButton.SetActive(false);
        }
    }

    void ActivateButton(GameObject button, UnityEngine.Events.UnityAction action)
    {
        button.SetActive(true);
        Button btnComponent = button.GetComponent<Button>();
        btnComponent.onClick.RemoveAllListeners();
        btnComponent.onClick.AddListener(action);
    }

    void OnButtonPressed(GameObject buttonObject, int buttonID)
    {
        Destroy(buttonObject);

        switch (buttonID)
        {
            case 1:
                saving.generalButtonDestroyed = true;
                break;
            case 2:
                saving.storyButton2Destroyed = true;
                break;
            case 3:
                saving.storyButton3Destroyed = true;
                break;
        }

        saving.SavePlayerData();
    }
}
