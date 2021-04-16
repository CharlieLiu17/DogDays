using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PestoMovement : NPC_Movement
{
    public Animator animator;
    // determines how fast we want pesto to be
    public float deceleration;
    public float minSpeed;
    
    // initial Speed
    public float power;
    public bool crying;

    // this is to determine if pesto is moving
    private bool isMoving = false;

    public bool questIsActive = false;


    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {

    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if (questIsActive)
        {
            animator.SetBool("isMoving", true);


            if (power > minSpeed) {
                power = power - (Time.deltaTime * deceleration);
            }
            // modify current position by tempVect
            transform.Translate(0, 0, power);
        } else
        {
            animator.SetBool("isMoving", false);
        }
        if (crying && GameManager.Instance.getNpcEngaged().Equals(this.gameObject))
        {
            GetComponent<AudioSource>().Play();
            crying = false;
        }
    }

}