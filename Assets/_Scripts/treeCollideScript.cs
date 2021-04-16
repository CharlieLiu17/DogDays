using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class treeCollideScript : MonoBehaviour
{
    private Collider other;
    // this is to check if it is pesto colliding with the treed
    public GameObject pesto;

    // Start is called before the first frame update
    void Start()
    {
    
    }

    // Update is called once per frame
    void Update()
    {
        // we will want to ensure that the quest is object

    }

    /// <summary>
    /// OnTriggerEnter is called when the Collider other enters the trigger.
    /// </summary>
    /// <param name="other">The other Collider involved in this collision.</param>
    void OnTriggerEnter(Collider other)
    {
        // if quest is active
        // if player hits -> winning code
        // if pesto hits -> reset as if nothing happened at all. Add sad text
        
        // We are storing what collides with the tree so we can check in race quest.
        this.other = other;    


        if (GameManager.Instance.getCurrentDog().Equals(other.gameObject) || other.Equals(pesto)) {
            // `Instance` effectively makes script into a static object.
            // this is triggering the Enter Region method on all active quests
            GameManager.Instance.TriggerQuestEvent(QuestEvent.ENTER_REGION);    
        }
    
        
    }

    public GameObject GetWhoEntered()
    {
        return other.gameObject;
    }
}
