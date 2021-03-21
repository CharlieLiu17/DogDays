using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class DialogueEntry
{
    public string displayText;
    public List<DialogueOption> options; // Each option is assigned to a button in the dialogue UI

    public DialogueHandler dialogueHandler; // The handler of the currently active dialogue

    public DialogueEntry(DialogueHandler dialogueHandler, XmlDocument xml)
    {
        this.dialogueHandler = dialogueHandler;
        this.displayText = xml.SelectSingleNode("DialogueEntry/DisplayText").InnerText;

        this.options = new List<DialogueOption>();
        XmlNodeList optionsNodes = xml.SelectNodes("DialogueEntry/Options/Option");
        foreach (XmlNode node in optionsNodes)
        {
            options.Add(new DialogueOption(dialogueHandler, node));
        }
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
