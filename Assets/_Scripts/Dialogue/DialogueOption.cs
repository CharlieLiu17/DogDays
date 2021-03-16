using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueOption
{
    public string displayText; // Text displayed on the corresponding option button

    // Dialogue entry that appears when this option is selected
    public string nextDialogueName;
    public DialogueEntry nextDialogue;

    public bool selectOnEnterPressed; // Whether this option will be automatically chosen when the Enter key is pressed. UIManager handles this
    public List<DialogueEvent> events; // Events that happen at the moment this option is selected
    public List<DialogueCondition> conditions; // Conditions for this option to be available

    public DialogueHandler dialogueHandler; // The handler of the currently active dialogue

    public DialogueOption(DialogueHandler dialogueHandler, string displayText = "Ok", List<DialogueEvent> events = null, DialogueEntry nextDialogue = null, 
        List<DialogueCondition> conditions = null, bool selectOnEnterPressed = false)
    {
        this.displayText = displayText;
        this.events = events;
        this.nextDialogue = nextDialogue;
        this.conditions = conditions;
        this.selectOnEnterPressed = selectOnEnterPressed;
        this.dialogueHandler = dialogueHandler;
    }

    // Constructor using string for next dialogue (to be used once XML is working)
    public DialogueOption(string displayText, DialogueHandler dialogueHandler, List<DialogueEvent> events = null, string nextDialogueName = null,
        List<DialogueCondition> conditions = null, bool selectOnEnterPressed = false)
    {
        this.displayText = displayText;
        this.events = events;
        this.nextDialogueName = nextDialogueName;
        this.conditions = conditions;
        this.selectOnEnterPressed = selectOnEnterPressed;
        this.dialogueHandler = dialogueHandler;
    }

    public void OnSelect()
    {
        // If any condition false, stop execution. This shouldn't ever happen since the button should be disabled, but hey
        if (!ConditionsMet())
        {
            return;
        }

        dialogueHandler.CurrentDialogue = nextDialogue;

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
