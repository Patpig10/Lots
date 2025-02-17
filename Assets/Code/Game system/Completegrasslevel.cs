using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Completegrasslevel : MonoBehaviour
{
    public Saving saving;
    public GameObject[] buttonsTosetactive;

    public void Update()
    {
        if (saving.levelUnlocked == 5 && saving.Grassemblem == true)
        {
            buttonsTosetactive[0].SetActive(true);

        }
    }
}
