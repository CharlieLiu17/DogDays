using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Cinemachine;

[RequireComponent(typeof(DialogueHandler))]
[RequireComponent(typeof(Collider))]
public class NPCDialogue : MonoBehaviour
{
    private bool playerIsWithinRange;
    private DialogueHandler dialogueHandler;
    private NPC_Movement hm;
    public CinemachineVirtualCamera vcam;

    private void Start()
    {
        dialogueHandler = GetComponent<DialogueHandler>();
        hm = GetComponent<NPC_Movement>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (gameObject.name.Equals("Pesto Please Don't Leave"))
        {
            Debug.Log(other.gameObject.Equals(GameManager.Instance.getCurrentDog()));
        }
        if (other.gameObject.Equals(GameManager.Instance.getCurrentDog()))
        {
            playerIsWithinRange = true;
            UIManager.Instance.DialogueInitiationTextShown = true;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (GameObject.ReferenceEquals(other.gameObject,GameManager.Instance.getCurrentDog()) && !hm.inDialogue)
        {
            playerIsWithinRange = true;
            UIManager.Instance.DialogueInitiationTextShown = true;
        } else
        {
            playerIsWithinRange = false;
            UIManager.Instance.DialogueInitiationTextShown = false;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.Equals(GameManager.Instance.getCurrentDog()))
        {
            playerIsWithinRange = false;
            UIManager.Instance.DialogueInitiationTextShown = false;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && playerIsWithinRange)
        {
            dialogueHandler.DisplayCurrentDialogue();
            UIManager.Instance.DialogueInitiationTextShown = false;
            hm.inDialogue = true;
            GameManager.Instance.freeLookScript.enabled = false;
            GameManager.Instance.setNpcEngaged(gameObject);
            GameManager.Instance.TriggerQuestEvent(QuestEvent.SPEAK_TO_NPC);
            vcam.enabled = true;
        }
    }
}
