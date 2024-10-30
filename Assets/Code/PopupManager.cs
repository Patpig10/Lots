using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PopupManager : MonoBehaviour
{
    public static PopupManager Instance; // Singleton instance
    public GameObject popupPanel; // Reference to the popup panel
    public TextMeshProUGUI popupText; // Reference to the popup text component

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ShowPopup(string message)
    {
        popupText.text = message;
        popupPanel.SetActive(true); // Show the popup panel
        Time.timeScale = 0; // Freeze the game
        StartCoroutine(HidePopupAfterDelay(2f)); // Hide the popup after 3 seconds
    }

    private IEnumerator HidePopupAfterDelay(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        popupPanel.SetActive(false); // Hide the popup panel
        Time.timeScale = 1; // Resume the game
    }
}
