using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement; // Needed to change scenes

public class Cutscene2 : MonoBehaviour
{
    [Header("Images")]
    public Image mapImage;
    public Image weaponsImage;
    public Image slimeImage;

    [Header("Text")]
    public TextMeshProUGUI storyText;

    [Header("Timing")]
    public float fadeDuration = 1.0f;
    public float textDisplayTime = 4.0f;

    [Header("Story Text")]
    public string[] textLines;

    private Coroutine cutsceneCoroutine;

    private void Start()
    {
        // Always start the cutscene on scene load
        cutsceneCoroutine = StartCoroutine(PlayCutscene());
    }

    public void SkipCutscene()
    {
        if (cutsceneCoroutine != null)
        {
            SceneManager.LoadScene("GameMAp");
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

        // Load Scene 2
        SceneManager.LoadScene("Scene2");
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

        // Teleport to Scene 2
        SceneManager.LoadScene("GameMAp");
    }

    IEnumerator FadeImage(Image image, float startAlpha, float endAlpha)
    {
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

        if (endAlpha == 0)
        {
            image.gameObject.SetActive(false);
        }
    }
}