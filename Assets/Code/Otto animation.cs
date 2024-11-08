using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ottoanimation : MonoBehaviour
{
    public Animator anim;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void fly1()
    {
        anim.SetTrigger("Fly1");
    }
}
