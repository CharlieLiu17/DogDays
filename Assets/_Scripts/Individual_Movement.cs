using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Individual_Movement : MonoBehaviour
{
    Transform thisTransform;
    Animator anim;
    NavMeshAgent agent;
    public float idleTime;
    public Transform[] myLocations;


    private int currentTransformIndex;
    private bool moveSwitch = false; // this variable is so animation setting only happens for one frame, reducing redundancy
    private bool moving = false; //this variable determines what animation to play
    private float timer;

    Transform currentLocation;

    // Start is called before the first frame update
    void Start()
    {
        thisTransform = GetComponent<Transform>();
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        currentTransformIndex = 0;
        //currentLocation = NPC_Manager.Instance.locations[currentTransformIndex
        //Debug.Log(currentTransformIndex);
    }

    // Update is called once per frame
    void Update()
    {
        //Giving movement directions
        if (!agent.hasPath) //only counts the timer when not moving
        {
            timer += Time.deltaTime;
        }
        if (timer >= idleTime)
        {
            //loop until it reaches a not occupied spot
            if (currentTransformIndex == myLocations.Length - 1)
            {
                currentTransformIndex = -1; // -1 + 1 reset back to 0
            }
            currentTransformIndex++;
            currentLocation = myLocations[currentTransformIndex];
            agent.SetDestination(currentLocation.position);
            timer = 0; //reset
            moveSwitch = true;
            moving = true;
        }

        if (!agent.pathPending)
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                {
                    moving = false;
                    moveSwitch = true;
                }
            }
        }
        //Setting the animation if walking
        if (moveSwitch)
        {
            if (moving)
            {
                anim.SetBool("Walking", true);
            }
            else
            {
                anim.SetBool("Walking", false);
            }
            moveSwitch = false;
        }
    }

    public void setCurrentTransformationIndex(int index)
    {
        currentTransformIndex = index;
    }

    public void setCurrentLocation(Transform location)
    {
        currentLocation = location;
    }

    public void setTransform(Transform transform)
    {
        thisTransform.position = transform.position;
    }
}

