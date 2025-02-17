using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GuideUpdater : MonoBehaviour
{
    public Saving Saving; // Reference to the Saving script
    public TextMeshProUGUI guideText; // Reference to the TMPro text component

    void Update()
    {
        UpdateGuide();
    }

    void UpdateGuide()
    {
        if (Saving.isShootUnlocked && Saving.levelUnlocked ==5)
        {
            guideText.text = "Return to the village.";
            return;
        }

        switch (Saving.levelUnlocked)
        {
            case 1:
                guideText.text = "Head to 1-1 (Click on the blue icon to enter the level).";
                break;
            case 2:
                guideText.text = "Head to the Village.";
                break;
            case 3:
                guideText.text = "Cross the river in 1-2.";
                break;
            case 4:
                guideText.text = "Navigate through the forest in 1-3.";
                break;
            case 5:
                guideText.text = "Defeat the boss.";
                break;
            default:
                guideText.text = "Explore and progress through the game.";
                break;
        }
    }
}