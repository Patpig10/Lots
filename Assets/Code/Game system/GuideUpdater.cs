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
        // Check for specific conditions first
        if (Saving.levelUnlocked == 7 && Saving.ice == true)
        {
            guideText.text = "Enter the ice palace by helping or fighting.";
            return;
        }

        if (Saving.levelUnlocked == 8 && Saving.ice == true && Saving.Iceemblem == false)
        {
            guideText.text = "Defeat the Queen.";
            return;
        }
        if (Saving.Iceemblem == true && Saving.Fireemblem == true)
        {
            guideText.text = "Head back to the village";
            return;
        }


        if (Saving.Iceemblem == true && Saving.Fireemblem == false && Saving.fire == false)
        {
            guideText.text = "Head to the Molten Core.";
            return;
        }

        if (Saving.Arena == true && Saving.fire == true && Saving.Fireemblem == false && Saving.levelUnlocked >= 6)
        {
            guideText.text = "Defeat the champion.";
            return;
        }

        // Default progression based on levelUnlocked
        if (Saving.isShootUnlocked && Saving.levelUnlocked == 5)
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
                guideText.text = "Choose where you want to continue (Ice caps or Molten Core).";
                break;
        }
    }
}