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

    [Header("Text")]
    public TextMeshProUGUI dialogueText;

    [Header("Timing")]
    public float fadeDuration = 1.0f;
    public float textDisplayTime = 4.0f;

    [Header("Canvases")]
    public GameObject battleCanvas;
    public GameObject townHallCanvas;

    private void Start()
    {
        if (townHallCanvas != null)
            townHallCanvas.SetActive(false);

        StartCoroutine(PlayCutscene());
    }

    IEnumerator PlayCutscene()
    {
        // Lich's last stand
        yield return ShowDialogue("Lich", "Ugh… Don’t think you’ve won just yet.");
        yield return ShowDialogue("Lich", "You pest!");
        yield return ShowDialogue("Lich", "This place… shall be your tomb!");

        // Lich summons spectral swords
        spectralSwordsImage.gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        yield return ShowDialogue("Lich", "What?! How… how is this possible?!");

        // Sword Hero appears
        owlSkeletonImage.gameObject.SetActive(true);
        yield return ShowDialogue("???", "Old friend… you, of all people, should know the answer to that.");
        yield return ShowDialogue("Lich", "No… It can’t be… You—you should be dead! Sword Hero!");
        swordHeroImage.gameObject.SetActive(true);

        yield return ShowDialogue("Sword Hero", "Slime, you’ve done well. Every battle you’ve fought, every challenge you’ve overcome… I have witnessed it all.");
        yield return ShowDialogue("Sword Hero", "You have the heart of a true hero.");

        yield return ShowDialogue("Lich", "Damn you… Damn you all!");
        yield return ShowDialogue("Sword Hero", "Lich, our tale ends here. Let this be the path for new legends to rise.");

        // Lich defeated, screen fades to white
        yield return StartCoroutine(FadeImage(lichImage, 1, 0));
        yield return StartCoroutine(FadeScreenToWhite());

        // Transition to town hall
        battleCanvas.SetActive(false);
        townHallCanvas.SetActive(true);
        townHallImage.gameObject.SetActive(true);

        // Town hall ceremony
        yield return ShowDialogue("Princess", "Today, we stand united in victory over the darkness that threatened our world.");
        yield return ShowDialogue("Princess", "We mourn those we lost, but through this battle, we have shattered the walls that once divided us.");
        yield return ShowDialogue("Princess", "And for that, we owe everything… to our slimy friend.");

        slimeImage.gameObject.SetActive(true);
        yield return ShowDialogue("Princess", "Slime, for your bravery in single-handedly defeating the Lich, and with the unanimous agreement of all rulers…");
        yield return ShowDialogue("Princess", "I hereby grant you the title of Slime Hero!");

        yield return ShowDialogue("Forest Queen", "I have no doubt… You will surpass even the Sword Hero himself.");
        yield return ShowDialogue("Flame Empress", "Hahaha! Next time, we fight! My hammer’s eager to test your strength.");
        yield return ShowDialogue("Ice Queen", "Hmph… If you ever need anything… You may come to me.");

        yield return ShowDialogue("The End", "");
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

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / fadeDuration);
            image.color = color;
            yield return null;
        }
    }

    IEnumerator FadeScreenToWhite()
    {
        Image fadeScreen = new GameObject("FadeScreen").AddComponent<Image>();
        fadeScreen.color = new Color(1, 1, 1, 0);
        fadeScreen.rectTransform.SetParent(townHallCanvas.transform, false);
        fadeScreen.rectTransform.anchorMin = Vector2.zero;
        fadeScreen.rectTransform.anchorMax = Vector2.one;
        fadeScreen.rectTransform.offsetMin = Vector2.zero;
        fadeScreen.rectTransform.offsetMax = Vector2.zero;

        yield return StartCoroutine(FadeImage(fadeScreen, 0, 1));
        yield return new WaitForSeconds(2f);
        Destroy(fadeScreen.gameObject);
    }
}
