using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
public class CursorManager : MonoBehaviour
{
    public static CursorManager Instance;
    public GameObject cursor;
    private Image virtualCursor;
    private void Awake()
    {
        // virtualCursor = cursor.GetComponent<Image>();

        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); // Avoid duplicate singletons
        }

        virtualCursor = cursor.GetComponent<Image>();

    }
    private void SetCursorAlpha(float alpha)
    {
        if (virtualCursor != null)
        {
            Color color = virtualCursor.color;
            color.a = alpha;
            virtualCursor.color = color;
        }
    }
    public void ShowCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SetCursorAlpha(1f); // Show the cursor
    }

    public void HideCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        SetCursorAlpha(0f); // Hide the cursor
    }
}
