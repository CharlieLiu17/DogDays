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
    private Human_Movement hm;
    public CinemachineVirtualCamera vcam;

    private void Start()
    {
        dialogueHandler = GetComponent<DialogueHandler>();
        hm = GetComponent<Human_Movement>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsWithinRange = true;
            UIManager.Instance.DialogueInitiationTextShown = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
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
            vcam.enabled = true;
        }
    }
}
