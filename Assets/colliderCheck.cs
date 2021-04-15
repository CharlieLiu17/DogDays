using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class colliderCheck : MonoBehaviour
{
    public GameObject theObject;
    private void OnTriggerStay(Collider other)
    {
        theObject = other.gameObject;
    }
}
