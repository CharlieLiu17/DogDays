using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DontDestroy : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        DontDestroyOnLoad(gameObject);
    }
    
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        DestroyThisObject();
    }
    public void DestroyThisObject()
    {
        /**
        GameObject anObject = GameObject.Find(gameObject.name);
        Debug.Log("Outside" + gameObject.name);
        if (anObject != null && !ReferenceEquals(anObject, gameObject))
        {
            Debug.Log("inside");
            Destroy(gameObject);
        } **/
    }
}
