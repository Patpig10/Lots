using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controllercontrol : MonoBehaviour
{
    public bool gamepad = false; // Variable to check if gamepad is used
    public GameObject conrollerinc; // Reference to the player GameObject
    public CursorManager cursor;
    public bool test;
    // Start is called before the first frame update
    void Start()
    {
        bool gamepad = cursor.Controller; // Check if the gamepad is used
    }

    // Update is called once per frame
    void Update()
    {
        if (cursor.Controller == true)
        {
            conrollerinc.SetActive(true); // Activate the player GameObject
        }
        else
        {
            conrollerinc.SetActive(false); // Deactivate the player GameObject
        }

        if(cursor.Controller == true && test == false)
        {
            OnEnable(); // Call OnEnable if the controller is connected
        }
        else if(gamepad == false && test == true)
        {
            OnDisable(); // Call OnDisable if the controller is not connected
        }
    }
public void OnEnable()
    {
        gamepad = true; // Set gamepad to true when the script is enabled
        test = false; // Set test to true when the script is disabled

    }
    public void OnDisable()
    {
        gamepad = false; // Set gamepad to false when the script is disabled
        test = true; // Set test to true when the script is disabled
    }


}
