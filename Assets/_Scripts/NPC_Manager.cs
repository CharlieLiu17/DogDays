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
    public Human_Movement[] npcs;

    public void Start()
    {
        for (int i = 0; i < npcs.Length; i++)
        {
            int currentTransformIndex = Random.Range(0, NPC_Manager.Instance.locations.Length);
            //int currentTransformIndex = i; for testing
            if (!locations[currentTransformIndex].isOccupied)
            {
                npcs[i].transform.position = locations[currentTransformIndex].transform.position;
                npcs[i].setCurrentTransformationIndex(currentTransformIndex);
                locations[currentTransformIndex].isOccupied = true;
            } else
            {
                i--; //this is the most shit code eever
            }
        }
    }

}

[System.Serializable]
public struct HumanLocation
{
    public Transform transform;
    public bool isOccupied;
}