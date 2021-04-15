using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class treeCollideScript : MonoBehaviour
{


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
        print("corg hit");
    }
}
