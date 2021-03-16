using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class DialogueEntry
{
    public string displayText;
    public List<DialogueOption> options; // Each option is assigned to a button in the dialogue UI

    public DialogueHandler dialogueHandler; // The handler of the currently active dialogue

    // Public constructor using XML
    public DialogueEntry(XmlNode node)
    {

    }

    public DialogueEntry(DialogueHandler dialogueHandler, string displayText, List<DialogueOption> options)
    {
        this.displayText = displayText;
        this.options = options;
        if (options == null)
        {
            this.options = new List<DialogueOption>();

            // If a DialogueEntry contains no options, give it an option that closes the dialogue UI
            List<DialogueEvent> events = new List<DialogueEvent>();
            events.Add(new DialogueEvent(DialogueEventType.EndDialogue));
            this.options.Add(new DialogueOption(dialogueHandler, "Ok", events));
        }
    }

    public override string ToString()
    {
        string stringToReturn = displayText + ": [";
        foreach (DialogueOption option in options) {
            stringToReturn += option.displayText + ", ";
        }
        stringToReturn.Remove(stringToReturn.Length - 1);
        stringToReturn += "]";
        return stringToReturn;
    }
}
