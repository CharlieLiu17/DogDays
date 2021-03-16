using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

// Attached to a dialogue source like an NPC and given a DialogueEntry.
public class DialogueHandler : MonoBehaviour
{
    private DialogueEntry _currentDialogue;

    [SerializeField]
    private string dialogueName;

    public DialogueEntry CurrentDialogue
    {
        get { return _currentDialogue; }
        set
        {
            _currentDialogue = value;
            if (_currentDialogue == null)
            {
                UIManager.Instance.DialoguePanelShown = false;
            } else
            {
                DisplayCurrentDialogue();
            }
        }
    }

    void Start()
    {
        List<DialogueOption> options = new List<DialogueOption>();

        DialogueEntry next = new DialogueEntry(this, "It worked! (That's a miracle!)", null);

        options.Add(new DialogueOption(this, "Test Option 1", null, next, null, true));

        CurrentDialogue = new DialogueEntry(this, "This is test dialogue.", options);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (_currentDialogue.options != null)
            {
                foreach (DialogueOption option in _currentDialogue.options)
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
        UIManager.Instance.DisplayDialogue(_currentDialogue);
    }
}
