using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Human_Movement : MonoBehaviour
{
    public Transform[] locations;
    public Transform self;
    public float idleTime;

    private bool moving;
    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        self = locations[Random.Range(0, locations.Length)];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
