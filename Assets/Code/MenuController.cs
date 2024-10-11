using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    public RectTransform dropdownMenu;  // Reference to the dropdown menu panel
    public RectTransform button;        // Reference to the button
    public float slideSpeed = 500f;     // Speed at which the menu will slide
    private bool isMenuOpen = false;    // Tracks whether the menu is open or closed
    private Vector2 closedPosition;     // Position when the menu is closed
    private Vector2 openPosition;       // Position when the menu is open

    void Start()
    {
        // Calculate the closed position: Ensure the menu is completely off-screen or hidden
        closedPosition = new Vector2(button.anchoredPosition.x + button.rect.width + dropdownMenu.rect.width, button.anchoredPosition.y);

        // Calculate the open position: Place it beside the button, fully visible
        openPosition = new Vector2(button.anchoredPosition.x + button.rect.width, button.anchoredPosition.y);

        // Set the initial position of the menu to be completely closed (off to the side)
        dropdownMenu.anchoredPosition = closedPosition;
    }

    public void ToggleMenu()
    {
        isMenuOpen = !isMenuOpen;
        StopAllCoroutines();  // Stop any ongoing movement to avoid conflicts
        StartCoroutine(SlideMenu(isMenuOpen));
    }

    private System.Collections.IEnumerator SlideMenu(bool open)
    {
        Vector2 targetPosition = open ? openPosition : closedPosition;

        // Continue sliding until the menu reaches its target position
        while (Vector2.Distance(dropdownMenu.anchoredPosition, targetPosition) > 0.1f)
        {
            // Lerp smoothly between current position and target position
            dropdownMenu.anchoredPosition = Vector2.Lerp(dropdownMenu.anchoredPosition, targetPosition, Time.deltaTime * 10f);
            yield return null; // wait for the next frame
        }

        // Ensure it exactly hits the target position
        dropdownMenu.anchoredPosition = targetPosition;
    }
}
