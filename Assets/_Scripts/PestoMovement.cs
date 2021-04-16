using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PestoMovement : NPC_Movement
{
    public Animator animator;
    public Rigidbody rigidbody;
    // determines how fast we want pesto to be
    public float speed;
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
        // immediatly get the rigidbody of Pesto
        rigidbody = GetComponent<Rigidbody>();

    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if (questIsActive)
        {
            animator.SetBool("isMoving", true);
            Vector3 tempVect = new Vector3(1, 0, 0);
            // apply our speed
            tempVect = tempVect.normalized * speed * Time.deltaTime;
            // modify current position by tempVect
            rigidbody.MovePosition(transform.position + tempVect);
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