using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JoystickCursor : MonoBehaviour
{
    public float speed = 1000f;
    public float deadzone = 0.2f;

    void Update()
    {
        float moveX = Input.GetAxis("Joystick Right X");
        float moveY = Input.GetAxis("Joystick Right Y");

        Vector2 move = new Vector2(moveX, moveY);

        if (move.magnitude > deadzone)
        {
            Vector3 currentPos = Input.mousePosition;
            Vector3 delta = new Vector3(move.x, move.y, 0) * speed * Time.deltaTime;
            Vector3 newPos = currentPos + delta;

            // Clamp to screen bounds
            newPos.x = Mathf.Clamp(newPos.x, 0, Screen.width);
            newPos.y = Mathf.Clamp(newPos.y, 0, Screen.height);

            Cursor.visible = true;  // Show cursor (or hide if needed)
            Cursor.lockState = CursorLockMode.None;
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto); // Optional

            // Set the new mouse position
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto); // Needed for some platforms
            SetMousePosition(newPos);
        }
    }


    void SetMousePosition(Vector3 pos)
    {
#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
        // Works on Windows
        var screenPos = new Vector2(pos.x, Screen.height - pos.y); // Flip Y
        //System.Windows.Forms.Cursor.Position = new System.Drawing.Point((int)screenPos.x, (int)screenPos.y);
#endif
    }
}