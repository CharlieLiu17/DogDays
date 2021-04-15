using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    private AudioSource audioSource;
    public float volumeLevel;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (PauseMenuFunctions.GameIsPaused)
        {
            audioSource.volume = 0.5f * volumeLevel;
        } else
        {

            audioSource.volume = volumeLevel;
        }
    }
}
