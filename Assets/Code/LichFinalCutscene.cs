using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LichFinalCutscene : MonoBehaviour
{
    [Header("Images")]
    public Image lichImage;
    public Image swordHeroImage;
    public Image slimeImage;
    public Image spectralSwordsImage;
    public Image owlSkeletonImage;
    public Image townHallImage;
    public Image endingImage; // Added for the ending picture

    [Header("Text")]
    public TextMeshProUGUI dialogueText;

    [Header("Timing")]
    public float fadeDuration = 1.0f;
    public float textDisplayTime = 4.0f;

    [Header("Music")]
    public AudioSource battleMusic; // Music during the battle
    public AudioSource townHallMusic; // Music during the town hall ceremony
    public AudioSource endingMusic; // Music during the ending

    [Header("Canvases")]
    public GameObject mainCanvas; // Single canvas for everything

    private void Start()
    {
        // Ensure the ending image is hidden at the start
        if (endingImage != null)
            endingImage.gameObject.SetActive(false);

        // Start the battle music
        if (battleMusic != null)
            battleMusic.Play();

        // Start the cutscene
        StartCoroutine(PlayCutscene());
    }

    IEnumerator PlayCutscene()
    {
        // Ensure the Lich image is visible at the start
        lichImage.gameObject.SetActive(true);

        // Lich's last stand
        yield return ShowDialogue("Lich", "Ugh… Don’t think you’ve won just yet.");
        yield return ShowDialogue("Lich", "You pest!");
        yield return ShowDialogue("Lich", "This place… shall be your tomb!");

        // Lich summons spectral swords
        spectralSwordsImage.gameObject.SetActive(true);
        lichImage.gameObject.SetActive(false);
        yield return new WaitForSeconds(1f);
        yield return ShowDialogue("Lich", "What?! How… how is this possible?!");

        // Sword Hero appears (using the Owl Skeleton image)
        owlSkeletonImage.gameObject.SetActive(true);
        spectralSwordsImage.gameObject.SetActive(false);

        yield return ShowDialogue("???", "Old friend… you, of all people, should know the answer to that.");
        yield return ShowDialogue("Lich", "No… It can’t be… You—you should be dead! Sword Hero!");

        yield return ShowDialogue("Sword Hero", "Slime, you’ve done well. Every battle you’ve fought, every challenge you’ve overcome… I have witnessed it all.");
        yield return ShowDialogue("Sword Hero", "You have the heart of a true hero.");

        yield return ShowDialogue("Lich", "Damn you… Damn you all!");
        yield return ShowDialogue("Sword Hero", "Lich, our tale ends here. Let this be the path for new legends to rise.");

        // Lich defeated, fade out the Lich image and deactivate it
        yield return StartCoroutine(FadeImage(lichImage, 1, 0)); // Fade out and deactivate

        // Fade the screen to white and show "3 Days Later" text
        yield return StartCoroutine(FadeScreenToWhite());

        // Transition to town hall
        townHallImage.gameObject.SetActive(true);
        owlSkeletonImage.gameObject.SetActive(false);


        // Switch to town hall music
        if (battleMusic != null)
            battleMusic.Stop();
        if (townHallMusic != null)
            townHallMusic.Play();

        // Town hall ceremony
        yield return ShowDialogue("Princess", "Today, we stand united in victory over the darkness that threatened our world.");
        yield return ShowDialogue("Princess", "We mourn those we lost, but through this battle, we have shattered the walls that once divided us.");
        yield return ShowDialogue("Princess", "And for that, we owe everything… to our slimy friend.");

        slimeImage.gameObject.SetActive(true);
        townHallImage.gameObject.SetActive(false);

        yield return ShowDialogue("Princess", "Slime, for your bravery in single-handedly defeating the Lich, and with the unanimous agreement of all rulers…");
        yield return ShowDialogue("Princess", "I hereby grant you the title of Slime Hero!");

        yield return ShowDialogue("Forest Queen", "I have no doubt… You will surpass even the Sword Hero himself.");
        yield return ShowDialogue("Flame Empress", "Hahaha! Next time, we fight! My hammer’s eager to test your strength.");
        yield return ShowDialogue("Ice Queen", "Hmph… If you ever need anything… You may come to me.");

        // Show the ending image
        endingImage.gameObject.SetActive(true);
        slimeImage.gameObject.SetActive(false);

        yield return StartCoroutine(FadeImage(endingImage, 0, 1)); // Fade in the ending image

        // Switch to ending music
        if (townHallMusic != null)
            townHallMusic.Stop();
        if (endingMusic != null)
            endingMusic.Play();

        // Wait for a moment before ending
        yield return new WaitForSeconds(5f);
    }

    IEnumerator ShowDialogue(string speaker, string text)
    {
        dialogueText.text = speaker + ": " + text;
        yield return new WaitForSeconds(textDisplayTime);
    }

    IEnumerator FadeImage(Image image, float startAlpha, float endAlpha)
    {
        float elapsedTime = 0f;
        Color color = image.color;
        color.a = startAlpha;
        image.color = color;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / fadeDuration);
            image.color = color;
            yield return null;
        }

        // Ensure the alpha is set to the final value
        color.a = endAlpha;
        image.color = color;

        // If fading out, deactivate the image
        if (endAlpha == 0)
            image.gameObject.SetActive(false);
    }

    IEnumerator FadeScreenToWhite()
    {
        // Create a white fade screen
        Image fadeScreen = new GameObject("FadeScreen").AddComponent<Image>();
        fadeScreen.color = new Color(1, 1, 1, 0);
        fadeScreen.rectTransform.SetParent(mainCanvas.transform, false);
        fadeScreen.rectTransform.anchorMin = Vector2.zero;
        fadeScreen.rectTransform.anchorMax = Vector2.one;
        fadeScreen.rectTransform.offsetMin = Vector2.zero;
        fadeScreen.rectTransform.offsetMax = Vector2.zero;

        // Create "3 Days Later" text
        TextMeshProUGUI daysLaterText = new GameObject("DaysLaterText").AddComponent<TextMeshProUGUI>();
        daysLaterText.text = "3 Days Later";
        daysLaterText.color = new Color(0, 0, 0, 0); // Start fully transparent
        daysLaterText.alignment = TextAlignmentOptions.Center;
        daysLaterText.fontSize = 48;
        daysLaterText.rectTransform.SetParent(mainCanvas.transform, false);
        daysLaterText.rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        daysLaterText.rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        daysLaterText.rectTransform.anchoredPosition = Vector2.zero;

        // Fade to white
        yield return StartCoroutine(FadeImage(fadeScreen, 0, 1));

        // Fade in "3 Days Later" text
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            daysLaterText.color = new Color(0, 0, 0, Mathf.Lerp(0, 1, elapsedTime / fadeDuration));
            yield return null;
        }

        // Ensure the text is fully visible
        daysLaterText.color = new Color(0, 0, 0, 1);

        // Wait for a moment
        yield return new WaitForSeconds(2f);

        // Destroy the fade screen and text
        Destroy(fadeScreen.gameObject);
        Destroy(daysLaterText.gameObject);
    }
}