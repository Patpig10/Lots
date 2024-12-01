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
        // Ensure the button and dropdownMenu are using the same anchor and pivot settings.
        dropdownMenu.pivot = new Vector2(0, 0.5f);  // Set pivot to be at the left-middle
        button.pivot = new Vector2(0, 0.5f);        // Set pivot of button to left-middle for consistency

        // Calculate the closed position: Ensure the menu is completely off-screen to the right
        closedPosition = new Vector2(button.localPosition.x + button.rect.width + dropdownMenu.rect.width, button.localPosition.y);

        // Calculate the open position: Place it exactly beside the button
        openPosition = new Vector2(button.localPosition.x + button.rect.width, button.localPosition.y);

        // Set the initial position of the menu to be completely closed (off to the side)
        dropdownMenu.localPosition = closedPosition;
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
        while (Vector2.Distance(dropdownMenu.localPosition, targetPosition) > 0.1f)
        {
            // Lerp smoothly between current position and target position
            dropdownMenu.localPosition = Vector2.Lerp(dropdownMenu.localPosition, targetPosition, Time.deltaTime * 10f);
            yield return null; // wait for the next frame
        }

        // Ensure it exactly hits the target position
        dropdownMenu.localPosition = targetPosition;
    }
}
