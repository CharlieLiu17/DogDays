using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using Cinemachine;

// Attached to a dialogue source like an NPC and given a DialogueEntry.
public class DialogueHandler : MonoBehaviour
{
    private DialogueEntry currentDialogue;

    private NPC_Movement hm;

    [SerializeField]
    private CinemachineVirtualCamera vcam;

    [SerializeField]
    private string _dialogueName;

    [SerializeField]
    private bool displayOnStart;

    public string DialogueName
    {
        get { return _dialogueName; }
        set
        {
            _dialogueName = value;
            if (_dialogueName == string.Empty || _dialogueName == null)
            {
                UIManager.Instance.DialoguePanelShown = false;
                GameManager.Instance.getCurrentDog().GetComponent<ThirdPersonMovement>().enabled = true;
                if (hm != null)
                {
                    hm.inDialogue = false;
                }
                if (vcam != null)
                {
                    vcam.enabled = false;
                }
                GameManager.Instance.freeLookScript.enabled = true;
                GameManager.Instance.CursorLocked = true;
                GameManager.Instance.setNpcEngaged(null);
            }
            else
            {
                GetCurrentDialogueFromXML();
                DisplayCurrentDialogue();
            }
        }
    }

    public void setDialogueName(string dialogueName)
    {
        _dialogueName = dialogueName;
    }

    private void GetCurrentDialogueFromXML()
    {
        // Will this work on all computers? I hope so D:
        TextAsset xmlText = Resources.Load<TextAsset>("XML/" + _dialogueName);
        if (xmlText == null)
        {
            Debug.LogError("DialogueHandler on " + gameObject +  ": Could not find Dialogue XML file with path \"XML/" + _dialogueName + "\"");
            return;
        }
        XmlDocument xml = new XmlDocument();
        xml.LoadXml(xmlText.text);
        currentDialogue = new DialogueEntry(this, xml);
    }

    void Start()
    {
        GetCurrentDialogueFromXML();
        hm = GetComponent<NPC_Movement>();
        
        if (displayOnStart)
        {
            DisplayCurrentDialogue();
            GameManager.Instance.CursorLocked = false;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && UIManager.Instance.CurrentlyDisplayedDialogue == currentDialogue)
        {
            if (currentDialogue.options != null)
            {
                foreach (DialogueOption option in currentDialogue.options)
                {
                    if (option.selectOnEnterPressed)
                    {
                        option.OnSelect();
                    }
                }
            }
        }
    }

    public void DisplayCurrentDialogue()
    {
        UIManager.Instance.DisplayDialogue(currentDialogue);
    }
}
