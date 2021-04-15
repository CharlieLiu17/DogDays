using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Footsteps : MonoBehaviour
{

    [SerializeField]
    private AudioClip walkingWood;

    [SerializeField]
    private AudioClip walkingGrass;
    [SerializeField]
    private AudioSource audioSource;

    
    public void Step()
    {
        RaycastHit hit;
        audioSource.pitch = 1 + Random.Range(-0.1f, 0.1f);
        //checks for ground underneath, 0.3 is to account for transform's low position
        if (Physics.Raycast(transform.position + new Vector3(0f, 0.3f, 0f), Vector3.down, out hit))
        {
            if (hit.transform.tag == "Wood") //the players .gameObject is there because i'm not sure if you have it set to a transform, if it's a GameObject then you can be rid of it :)
            {

                audioSource.PlayOneShot(walkingWood, 0.05f);
            } else if (hit.transform.tag == "Grass")
            {
                audioSource.PlayOneShot(walkingGrass, 0.05f);
            }
        }
    }
}
