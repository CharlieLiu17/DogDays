using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Human_Movement : MonoBehaviour
{
    Transform transform;
    Animator anim;
    NavMeshAgent agent;
    public float idleTime;

    
    private int currentTransformIndex;
    private bool moveSwitch = false; // this variable is so animation setting only happens for one frame, reducing redundancy
    private bool moving = false; //this variable determines what animation to play
    private float timer;

    HumanLocation currentLocation;

    // Start is called before the first frame update
    void Start()
    {
        transform = GetComponent<Transform>();
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        currentTransformIndex = Random.Range(0, NPC_Manager.Instance.locations.Length);
        currentLocation = NPC_Manager.Instance.locations[currentTransformIndex];
        transform.position = currentLocation.transform.position;
        currentLocation.isOccupied = true;
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
            currentLocation.isOccupied = false;
            //loop until it reaches a not occupied spot
            if (currentTransformIndex == NPC_Manager.Instance.locations.Length - 1)
            {
                currentTransformIndex = -1; // -1 + 1 reset back to 0
            }
            while (NPC_Manager.Instance.locations[currentTransformIndex + 1].isOccupied == true)
            {
                if (currentTransformIndex == NPC_Manager.Instance.locations.Length - 1)
                {
                    currentTransformIndex = -1; // -1 + 1 reset back to 0
                }
                currentTransformIndex++;
            }
            currentLocation = NPC_Manager.Instance.locations[++currentTransformIndex];
            agent.SetDestination(currentLocation.transform.position);
            currentLocation.isOccupied = true;
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
}
