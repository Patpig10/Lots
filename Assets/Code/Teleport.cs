using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    public Transform teleportTarget;
    public GameObject player;

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("triggered");
        player.transform.position = teleportTarget.transform.position;
    }
}
