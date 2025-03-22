using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pushanimation : MonoBehaviour
{
    public GameObject Player;
    public Animator PlayerAnimator;
    // Start is called before the first frame update
    void Start()
    {
        //Look for gameobject with the name Slime Knight
        Player = GameObject.Find("Slime Knight");
        //Get Slime Knight Animator
        PlayerAnimator = Player.GetComponent<Animator>();
    }
    public void push()
    {
        PlayerAnimator.SetTrigger("Push");
    }
}
   