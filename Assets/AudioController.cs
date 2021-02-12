using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public AudioSource audio;
    public float volumeLevel;

    // Start is called before the first frame update
    void Start()
    {
        audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (PauseMenuFunctions.GameIsPaused)
        {
            audio.volume = 0.5f * volumeLevel;
        } else
        {

            audio.volume = volumeLevel;
        }
    }
}
