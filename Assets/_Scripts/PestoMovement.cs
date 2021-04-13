using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PestoMovement : MonoBehaviour 
{
    public Animator animator;
    public Rigidbody rigidbody;
    // determines how fast we want pesto to be
    public float speed;

    // this is to determine if pesto is moving
    private bool isMoving = false;


    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        // immediatly get the rigidbody of Pesto
        rigidbody = GetComponent<Rigidbody>();

    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        animator.SetBool("isMoving", true);
        Vector3 tempVect = new Vector3(0, 0, 1);
        // apply our speed
        tempVect = tempVect.normalized * speed * Time.deltaTime;
        // modify current position by tempVect
        rigidbody.AddForce(tempVect);
    }

}