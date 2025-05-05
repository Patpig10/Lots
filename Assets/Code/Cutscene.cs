using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Cutscene : MonoBehaviour
{
    [Header("Images")]
    public Image mapImage; // Assign the map image in the Inspector
    public Image weaponsImage; // Assign the weapons image in the Inspector
    public Image slimeImage; // Assign the slime image in the Inspector

    [Header("Text")]
    public TextMeshProUGUI storyText; // Assign the TextMeshProUGUI component in the Inspector

    [Header("Timing")]
    public float fadeDuration = 1.0f; // Duration of fade-in/out
    public float textDisplayTime = 4.0f; // Time to display each text

    [Header("Story Text")]
    public string[] textLines; // Assign the 6 lines of text in the Inspector

    [Header("Canvases")]
    public GameObject canvasOne; // Assign Canvas One in the Inspector
    public GameObject canvasTwo; // Assign Canvas Two in the Inspector
    private Coroutine cutsceneCoroutine; // Reference to the running cutscene coroutine

    // Static variable to track if the cutscene has been played in the current session
    private static bool hasCutscenePlayed = false;

    private void Start()
    {
        // Ensure Canvas Two is inactive at the start
        if (canvasTwo != null)
        {
            canvasTwo.SetActive(false);
        }

        // Check if the cutscene has already been played
        if (!hasCutscenePlayed)
        {
            // Start the cutscene and store the reference
            cutsceneCoroutine = StartCoroutine(PlayCutscene());
            hasCutscenePlayed = true; // Mark the cutscene as played
        }
        else
        {
            // If the cutscene has already been played, skip to Canvas Two
            SkipCutscene();
        }
    }
    public void SkipCutscene()
    {
        // If the cutscene is playing
        if (cutsceneCoroutine != null)
        {
            StopCoroutine(cutsceneCoroutine);
            cutsceneCoroutine = null;
        }

        // Immediately fade out all images
        SetImageAlpha(mapImage, 0);
        SetImageAlpha(weaponsImage, 0);
        SetImageAlpha(slimeImage, 0);

        // Hide all images
        mapImage.gameObject.SetActive(false);
        weaponsImage.gameObject.SetActive(false);
        slimeImage.gameObject.SetActive(false);

        // Clear the text
        storyText.text = "";

        // Switch to Canvas Two
        if (canvasOne != null)
        {
            canvasOne.SetActive(false);
        }
        if (canvasTwo != null)
        {
            canvasTwo.SetActive(true);
        }
    }

    private void SetImageAlpha(Image image, float alpha)
    {
        if (image != null)
        {
            Color color = image.color;
            color.a = alpha;
            image.color = color;
        }
    }

    IEnumerator PlayCutscene()
    {
        // Fade in the map image
        yield return StartCoroutine(FadeImage(mapImage, 0, 1));

        // Display the first two lines of text
        storyText.text = textLines[0];
        yield return new WaitForSeconds(textDisplayTime);

        storyText.text = textLines[1];
        yield return new WaitForSeconds(textDisplayTime);

        // Fade out the map image
        yield return StartCoroutine(FadeImage(mapImage, 1, 0));

        // Fade in the weapons image
        yield return StartCoroutine(FadeImage(weaponsImage, 0, 1));

        // Display the next two lines of text
        storyText.text = textLines[2];
        yield return new WaitForSeconds(textDisplayTime);

        storyText.text = textLines[3];
        yield return new WaitForSeconds(textDisplayTime);

        // Fade out the weapons image
        yield return StartCoroutine(FadeImage(weaponsImage, 1, 0));

        // Fade in the slime image
        yield return StartCoroutine(FadeImage(slimeImage, 0, 1));

        // Display the final two lines of text
        storyText.text = textLines[4];
        yield return new WaitForSeconds(textDisplayTime);

        storyText.text = textLines[5];
        yield return new WaitForSeconds(textDisplayTime);

        // Fade out the slime image
        yield return StartCoroutine(FadeImage(slimeImage, 1, 0));

        // End of cutscene
        Debug.Log("Cutscene finished!");

        // Deactivate Canvas One and activate Canvas Two
        if (canvasOne != null)
        {
            canvasOne.SetActive(false);
        }
        if (canvasTwo != null)
        {
            canvasTwo.SetActive(true);
        }
    }

    IEnumerator FadeImage(Image image, float startAlpha, float endAlpha)
    {
        // Enable the image if it's not already
        if (!image.gameObject.activeSelf)
        {
            image.gameObject.SetActive(true);
        }

        float elapsedTime = 0f;
        Color color = image.color;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / fadeDuration);
            image.color = color;
            yield return null;
        }

        // Disable the image if it's fully faded out
        if (endAlpha == 0)
        {
            image.gameObject.SetActive(false);
        }
    }
}