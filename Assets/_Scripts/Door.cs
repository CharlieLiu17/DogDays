using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField]
    private int nextBuildIndex; // the next scene's build index that you want to go to
    [SerializeField]
    private Transform[] nextTransforms;

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.Equals(GameManager.Instance.getCurrentDog()))
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                UIManager.Instance.DoorOpenTextShown = false;
                GameManager.Instance.LoadNextScene(nextBuildIndex, nextTransforms);
            }
        } else
        {
            UIManager.Instance.DoorOpenTextShown = false;
        }
    }

    // If both dogs walk into collection range and one exits, the text will no longer appear. I'm not quite sure how to solve this yet.
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.Equals(GameManager.Instance.getCurrentDog())) // Both the dogs have the Player tag
        {
            UIManager.Instance.DoorOpenTextShown = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.Equals(GameManager.Instance.getCurrentDog())) // Both the dogs have the Player tag
        {
            UIManager.Instance.DoorOpenTextShown = false;
        }
    }
}
