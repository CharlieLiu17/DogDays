using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Human_Movement : MonoBehaviour
{
    Transform thisTransform;
    Animator anim;
    NavMeshAgent agent;
    public float idleTime;
    public bool inDialogue; 

    
    private int currentTransformIndex;
    private bool moveSwitch = false; // this variable is so animation setting only happens for one frame, reducing redundancy
    private bool moving = false; //this variable determines what animation to play
    private float timer;

    HumanLocation currentLocation;

    // Start is called before the first frame update
    void Start()
    {
        thisTransform = GetComponent<Transform>();
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        /**currentTransformIndex = Random.Range(0, NPC_Manager.Instance.locations.Length);
        //currentLocation = NPC_Manager.Instance.locations[currentTransformIndex];
        thisTransform.position = NPC_Manager.Instance.locations[currentTransformIndex].transform.position;
        //Debug.Log(currentTransformIndex);
        NPC_Manager.Instance.locations[currentTransformIndex].isOccupied = true;**/
    }

    // Update is called once per frame
    void Update()
    {
        if (!inDialogue)
        {
            //Giving movement directions
            if (!agent.hasPath) //only counts the timer when not moving
            {
                timer += Time.deltaTime;
            } else {
            }
            if (timer >= idleTime)
            {
                Debug.Log(currentTransformIndex);
                NPC_Manager.Instance.locations[currentTransformIndex].isOccupied = false;
                //loop until it reaches a not occupied spot
                if (currentTransformIndex == NPC_Manager.Instance.locations.Length - 1)
                {
                    currentTransformIndex = -1; // -1 + 1 reset back to 0
                }
                currentTransformIndex++;
                while (NPC_Manager.Instance.locations[currentTransformIndex].isOccupied)
                {
                    Debug.Log(currentTransformIndex + name);
                    if (currentTransformIndex == NPC_Manager.Instance.locations.Length - 1)
                    {
                        currentTransformIndex = -1; // -1 + 1 reset back to 0
                    }
                    currentTransformIndex++;
                }
                currentLocation = NPC_Manager.Instance.locations[currentTransformIndex];
                agent.SetDestination(currentLocation.transform.position);
                NPC_Manager.Instance.locations[currentTransformIndex].isOccupied = true;
                timer = 0; //reset
                moveSwitch = true;
                moving = true;
            }
        } else
        {
            agent.SetDestination(gameObject.transform.position);
            gameObject.transform.LookAt(GameObject.Find(GameManager.Instance.currentDog.ToString()).transform);
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

    public void setCurrentLocation(HumanLocation location)
    {
        currentLocation = location;
    }

    public void setTransform(Transform transform)
    {
        thisTransform.position = transform.position;
    }
}
