using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCplanner : MonoBehaviour
{
    public Saving Saving;
    public GameObject[] Grass1;
    public GameObject[] Grass2;
    public GameObject[] Grass3;
    public GameObject[] Ice1;
    public GameObject[] Ice2;
    public GameObject[] Fire1;
    public GameObject[] Fire2;
    public GameObject[] defectarmy;
    public GameObject[] Icearmy;
    public GameObject[] Firearmy;

    // Start is called before the first frame update
    void Start()
    {
        Saving = GameObject.Find("Saving").GetComponent<Saving>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Saving.levelUnlocked >= 1 && Saving.levelUnlocked <= 3)
        {
            foreach (GameObject go in Grass1)
            {
                go.SetActive(true);
            }
        }
        if (Saving.levelUnlocked >= 3 && Saving.levelUnlocked <= 4)
        {
            foreach (GameObject go in Grass2)
            {
                go.SetActive(true);
            }

            
        }

        if (Saving.Grassemblem == true)
        {
            foreach (GameObject go in defectarmy)
            {
                go.SetActive(true);
            }
        }

        if (Saving.levelUnlocked >= 4 && Saving.levelUnlocked <= 6)
        {
            foreach (GameObject go in Grass3)
            {
                go.SetActive(true);
            }
            
        }

        if (Saving.levelUnlocked >= 6)
        {
            foreach (GameObject go in Ice1)
            {
                go.SetActive(true);
            }
            foreach (GameObject go in Fire1)
            {
                go.SetActive(true);
            }
          
        }
        if (Saving.Iceemblem == true)
        {

            foreach (GameObject go in Ice2)
            {
                go.SetActive(true);
            }
            foreach (GameObject go in Ice1)
            {
                go.SetActive(false);
            }
            foreach (GameObject go in Icearmy)
            {
                go.SetActive(true);
            }

        }

     
        if (Saving.Fireemblem == true)
        {
            foreach (GameObject go in Fire2)
            {
                go.SetActive(true);
            }
            foreach (GameObject go in Fire1)
            {
                go.SetActive(true);

            }
            foreach (GameObject go in Firearmy)
            {
                go.SetActive(true);

            }
        }
    }
}
