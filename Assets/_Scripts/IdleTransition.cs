using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleTransition : MonoBehaviour
{
    public Animator anim;
    public float timeLimitConst;
    private float time = 0;
    private int count = 0;
    private float timeLimit;

    private void Start()
    {
        timeLimit = timeLimitConst + Random.Range(-3f, 3f);
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (count == 1)
        {
            anim.SetBool("IdleTransition", false);
            count = 0;
        }
        if (time >= timeLimit)
        {
            anim.SetBool("IdleTransition", true);
            Debug.Log(time);
            time = 0;
            count++;
            timeLimit = timeLimitConst + Random.Range(-3f, 3f);
        }
        
    }
}
