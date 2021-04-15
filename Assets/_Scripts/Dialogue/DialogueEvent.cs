using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

// It might be better to make each event type its own subclass, but that feels unneeded with the amount of event I can currently think of.
public class DialogueEvent
{
    public DialogueEventType type;

    public Item itemEventType;
    public int itemEventQuantity;

    public int questEventID;

    public DialogueEvent(XmlNode eventNode)
    {
        this.type = (DialogueEventType) Enum.Parse(typeof(DialogueEventType), eventNode.SelectSingleNode("Type").InnerText);
        if (type == DialogueEventType.GiveQuest)
        {
            if (eventNode.SelectSingleNode("QuestID") == null)
            {
                Debug.LogError("DialogueEvent of type GiveQuest did not have a QuestID specified!");
            }
            Debug.Log("wowowow");
            questEventID = int.Parse(eventNode.SelectSingleNode("QuestID").InnerText);
        }
    }

    public DialogueEvent(DialogueEventType type)
    {
        this.type = type;
    }
    public DialogueEvent(DialogueEventType type, Item itemEventType, int itemEventQuantity)
    {
        this.type = type;
        this.itemEventType = itemEventType;
        this.itemEventQuantity = itemEventQuantity;
    }
    public DialogueEvent(DialogueEventType type, int questEventID)
    {
        this.type = type;
        this.questEventID = questEventID;
    }

    public void Trigger()
    {
        switch (type)
        {
            case DialogueEventType.EndDialogue:
                UIManager.Instance.DisplayDialogue(null); // Null entry closes the window
                break;
            case DialogueEventType.GiveQuest:
                GameManager.Instance.AddQuest(Reference.Instance.GetQuestByID(questEventID));
                GameManager.Instance.TriggerQuestEvent(QuestEvent.OBTAIN_ITEM);
                break;
            case DialogueEventType.GiveItem:
                GameManager.Instance.AddItemToInventory(new InventoryItem(itemEventType, itemEventQuantity));
                break;
            default:
                Debug.Log("No event type defined!");
                break;
        }
        GameManager.Instance.getCurrentDog().GetComponent<ThirdPersonMovement>().enabled = true;
    }
}

public enum DialogueEventType
{
    EndDialogue,
    GiveItem,
    GiveQuest
}
