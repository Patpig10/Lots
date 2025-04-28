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
    public Image endingImage;

    [Header("Text")]
    public TextMeshProUGUI dialogueText;

    [Header("Timing")]
    public float fadeDuration = 1.0f;
    public float textDisplayTime = 4.0f;

    [Header("Music")]
    public AudioSource battleMusic;
    public AudioSource townHallMusic;
    public AudioSource endingMusic;

    [Header("Canvases")]
    public GameObject mainCanvas;

    private Coroutine cutsceneCoroutine;
    private bool skipRequested = false;

    private void Start()
    {
        if (endingImage != null)
            endingImage.gameObject.SetActive(false);

        if (battleMusic != null)
            battleMusic.Play();

        cutsceneCoroutine = StartCoroutine(PlayCutscene());
    }

    public void skipre()
    {

        if (!skipRequested)
        {
            skipRequested = true;
            if (cutsceneCoroutine != null)
            {
                StopCoroutine(cutsceneCoroutine);
                StartCoroutine(SkipToEnding());
            }
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S)) // Press 'S' to skip
        {
            if (!skipRequested)
            {
                skipRequested = true;
                if (cutsceneCoroutine != null)
                {
                    StopCoroutine(cutsceneCoroutine);
                    StartCoroutine(SkipToEnding());
                }
            }
        }
    }

    IEnumerator PlayCutscene()
    {
        lichImage.gameObject.SetActive(true);

        yield return ShowDialogue("Lich", "Ugh… Don’t think you’ve won just yet.");
        yield return ShowDialogue("Lich", "You pest!");
        yield return ShowDialogue("Lich", "This place… shall be your tomb!");

        spectralSwordsImage.gameObject.SetActive(true);
        lichImage.gameObject.SetActive(false);
        yield return new WaitForSeconds(1f);
        yield return ShowDialogue("Lich", "What?! How… how is this possible?!");

        owlSkeletonImage.gameObject.SetActive(true);
        spectralSwordsImage.gameObject.SetActive(false);

        yield return ShowDialogue("???", "Old friend… you, of all people, should know the answer to that.");
        yield return ShowDialogue("Lich", "No… It can’t be… You—you should be dead! Sword Hero!");
        yield return ShowDialogue("Sword Hero", "Slime, you’ve done well. Every battle you’ve fought, every challenge you’ve overcome… I have witnessed it all.");
        yield return ShowDialogue("Sword Hero", "You have the heart of a true hero.");
        yield return ShowDialogue("Lich", "Damn you… Damn you all!");
        yield return ShowDialogue("Sword Hero", "Lich, our tale ends here. Let this be the path for new legends to rise.");

        yield return StartCoroutine(FadeImage(lichImage, 1, 0));
        yield return StartCoroutine(FadeScreenToWhite());

        townHallImage.gameObject.SetActive(true);
        owlSkeletonImage.gameObject.SetActive(false);

        if (battleMusic != null)
            battleMusic.Stop();
        if (townHallMusic != null)
            townHallMusic.Play();

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

        endingImage.gameObject.SetActive(true);
        slimeImage.gameObject.SetActive(false);

        yield return StartCoroutine(FadeImage(endingImage, 0, 1));

        if (townHallMusic != null)
            townHallMusic.Stop();
        if (endingMusic != null)
            endingMusic.Play();

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

        color.a = endAlpha;
        image.color = color;

        if (endAlpha == 0)
            image.gameObject.SetActive(false);
    }

    IEnumerator FadeScreenToWhite()
    {
        Image fadeScreen = new GameObject("FadeScreen").AddComponent<Image>();
        fadeScreen.color = new Color(1, 1, 1, 0);
        fadeScreen.rectTransform.SetParent(mainCanvas.transform, false);
        fadeScreen.rectTransform.anchorMin = Vector2.zero;
        fadeScreen.rectTransform.anchorMax = Vector2.one;
        fadeScreen.rectTransform.offsetMin = Vector2.zero;
        fadeScreen.rectTransform.offsetMax = Vector2.zero;

        TextMeshProUGUI daysLaterText = new GameObject("DaysLaterText").AddComponent<TextMeshProUGUI>();
        daysLaterText.text = "3 Days Later";
        daysLaterText.color = new Color(0, 0, 0, 0);
        daysLaterText.alignment = TextAlignmentOptions.Center;
        daysLaterText.fontSize = 48;
        daysLaterText.rectTransform.SetParent(mainCanvas.transform, false);
        daysLaterText.rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        daysLaterText.rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        daysLaterText.rectTransform.anchoredPosition = Vector2.zero;

        yield return StartCoroutine(FadeImage(fadeScreen, 0, 1));

        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            daysLaterText.color = new Color(0, 0, 0, Mathf.Lerp(0, 1, elapsedTime / fadeDuration));
            yield return null;
        }

        daysLaterText.color = new Color(0, 0, 0, 1);

        yield return new WaitForSeconds(2f);

        Destroy(fadeScreen.gameObject);
        Destroy(daysLaterText.gameObject);
    }

    IEnumerator SkipToEnding()
    {
        // Clean up: deactivate all images
        lichImage.gameObject.SetActive(false);
        swordHeroImage.gameObject.SetActive(false);
        slimeImage.gameObject.SetActive(false);
        spectralSwordsImage.gameObject.SetActive(false);
        owlSkeletonImage.gameObject.SetActive(false);
        townHallImage.gameObject.SetActive(false);

        // Stop any playing music
        if (battleMusic != null)
            battleMusic.Stop();
        if (townHallMusic != null)
            townHallMusic.Stop();

        // Set the ending screen
        endingImage.gameObject.SetActive(true);
        endingImage.color = new Color(1, 1, 1, 1);

        // Change music to ending music
 
            endingMusic.Play();

        // Show final text
        dialogueText.text = "The End.";

        yield return null;
    }
}
