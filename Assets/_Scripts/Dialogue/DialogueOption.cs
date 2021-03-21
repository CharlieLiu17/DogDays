using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class DialogueOption
{
    public string displayText; // Text displayed on the corresponding option button

    // Dialogue entry that appears when this option is selected
    public DialogueEntry nextDialogueEntry;
    public string nextDialogue;

    public bool selectOnEnterPressed; // Whether this option will be automatically chosen when the Enter key is pressed. UIManager handles this
    public List<DialogueEvent> events; // Events that happen at the moment this option is selected
    public List<DialogueCondition> conditions; // Conditions for this option to be available

    public DialogueHandler dialogueHandler; // The handler of the currently active dialogue

    public DialogueOption(DialogueHandler dialogueHandler, string displayText = "Ok", List<DialogueEvent> events = null, DialogueEntry nextDialogueEntry = null, 
        List<DialogueCondition> conditions = null, bool selectOnEnterPressed = false)
    {
        this.displayText = displayText;
        this.events = events;
        this.nextDialogueEntry = nextDialogueEntry;
        this.conditions = conditions;
        this.selectOnEnterPressed = selectOnEnterPressed;
        this.dialogueHandler = dialogueHandler;
    }

    public DialogueOption(DialogueHandler dialogueHandler, XmlNode node)
    {
        this.dialogueHandler = dialogueHandler;

        displayText = node.SelectSingleNode("DisplayText").InnerText;
        selectOnEnterPressed = XmlConvert.ToBoolean(node.SelectSingleNode("SelectOnEnterPressed").InnerText);
        nextDialogue = node.SelectSingleNode("NextDialogue").InnerText;
    }

    public void OnSelect()
    {
        // If any condition false, stop execution. This shouldn't ever happen since the button should be disabled, but hey
        if (!ConditionsMet())
        {
            return;
        }

        dialogueHandler.DialogueName = nextDialogue;

        if (events != null)
        {
            foreach (DialogueEvent e in events)
            {
                e.Trigger();
            }
        }
    }

    public bool ConditionsMet()
    {
        if (conditions == null)
        {
            return true;
        }

        bool conditionsPassed = true;
        foreach (DialogueCondition c in conditions)
        {
            conditionsPassed = c.GetValue();
        }
        return conditionsPassed;
    }
}
