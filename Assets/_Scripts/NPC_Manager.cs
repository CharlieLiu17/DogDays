using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPC_Manager : MonoBehaviour
{
    #region Singleton Code
    private static NPC_Manager _instance;

    public static NPC_Manager Instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Debug.LogWarning("Attempted to Instantiate multiple NPC_Manager objects in one scene!");
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

    public HumanLocation[] locations;
    
}

[System.Serializable]
public struct HumanLocation
{
    public Transform transform;
    public bool isOccupied;
}