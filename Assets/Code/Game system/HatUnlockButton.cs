using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HatUnlockButton : MonoBehaviour
{
    public HatManager hatManager;
    public GameObject Default;
    public GameObject Crown;
    public GameObject Forest;
    public GameObject Funny;

    public GameObject Defaultshadow;
    public GameObject Crownshadow;
    public GameObject Forestshadow;
    public GameObject Funnyshadow;

    public void Awake()
    {
        hatManager = GameObject.Find("Player").GetComponent<HatManager>();
    }
    public void Update()
    {
        if (hatManager.havedefault == false)
        {
            Defaultshadow.SetActive(true);
            Default.SetActive(false);
        }
        else
        {
            Defaultshadow.SetActive(false);
            Default.SetActive(true);
        }

        if (hatManager.havecrown == false)
        {
            Crownshadow.SetActive(true);
            Crown.SetActive(false);
        }
        else
        {
            Crownshadow.SetActive(false);
            Crown.SetActive(true);
        }

        if (hatManager.haveforest == false)
        {
            Forestshadow.SetActive(true);
            Forest.SetActive(false);
        }
        else
        {
            Forestshadow.SetActive(false);
            Forest.SetActive(true);
        }

        if (hatManager.havefunny == false)
        {
            Funnyshadow.SetActive(true);
            Funny.SetActive(false);
        }
        else
        {
            Funnyshadow.SetActive(false);
            Funny.SetActive(true);
        }


    }

    public void wearhat(string hatName)
    {
        hatManager.WearHat(hatName);
    }

}
