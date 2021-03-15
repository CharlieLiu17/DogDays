using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogAudio : MonoBehaviour
{
    #region Singleton Code
    private static DogAudio _instance;

    public static DogAudio Instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Debug.LogWarning("Attempted to Instantiate multiple DogAudio objects in one scene!");
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    private void OnDestroy()
    {
        if (this == _instance) { _instance = null; }
    }
    #endregion


}
