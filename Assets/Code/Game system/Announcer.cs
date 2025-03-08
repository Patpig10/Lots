using UnityEngine;
using TMPro;
using System.Collections;

public class Announcer : MonoBehaviour
{
    public TextMeshProUGUI announcerText; // Use TextMeshProUGUI instead of Text
    public float displayTime = 3f;
    public GameObject announcerPanel;
    public void ShowMessage(string message)
    {
        StartCoroutine(DisplayMessage(message));
    }

    IEnumerator DisplayMessage(string message)
    {
        announcerText.text = message;
        announcerText.gameObject.SetActive(true);
        announcerPanel.gameObject.SetActive(true);
        yield return new WaitForSeconds(displayTime);
        announcerText.gameObject.SetActive(false);
        announcerPanel.gameObject.SetActive(false);
    }
}