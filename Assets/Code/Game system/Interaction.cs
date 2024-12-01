using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interaction : MonoBehaviour
{
    public UnityEvent interact;
    private void OnTriggerEnter(Collider other)
    {

        Debug.Log("I interacted with " + other.gameObject.tag);
        if (other.gameObject.tag == "Player")
            interact.Invoke();
    }
}