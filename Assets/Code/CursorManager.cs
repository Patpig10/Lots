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
    public bool Controller;

    private bool isControllerConnected;
    public bool titleScreen = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject); // Optional
        }
        else
        {
            return;
        }

        if (cursor == null)
        {
            cursor = GameObject.Find("Cursor");
        }

        if (cursor != null)
        {
            virtualCursor = cursor.GetComponent<Image>();
        }
        else
        {
            Debug.LogWarning("Cursor GameObject not found!");
        }

        isControllerConnected = Input.GetJoystickNames().Any(name => !string.IsNullOrEmpty(name));
        Controller = isControllerConnected;

        UpdateCursorVisibility();
    }

    private void Update()
    {
        bool controllerNowConnected = Input.GetJoystickNames().Any(name => !string.IsNullOrEmpty(name));

        if (controllerNowConnected != isControllerConnected)
        {
            isControllerConnected = controllerNowConnected;
            Controller = isControllerConnected;

            UpdateCursorVisibility();
        }
    }

    private void UpdateCursorVisibility()
    {
        if (Controller)
        {
            // Controller connected → Hide system cursor, show virtual
            Cursor.visible = false;
            SetCursorAlpha(1f);
        }
        else
        {
            // No controller → Show system cursor, hide virtual
            Cursor.visible = true;
            SetCursorAlpha(0f);

        }
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
        if (Controller)
        {
            SetCursorAlpha(1f);
        }
        else
        {
            Cursor.visible = true;
        }
    }

    public void HideCursor()
    {
        if (Controller)
        {
            SetCursorAlpha(0f);
        }
        else
        {
            Cursor.visible = false;
        }
    }
}
