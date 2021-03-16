using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// It might be better to make each event type its own subclass, but that feels unneeded with the amount of event I can currently think of.
public class DialogueEvent
{
    public DialogueEventType type;

    public Item itemEventType;
    public int itemEventQuantity;

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

    public void Trigger()
    {
        switch (type)
        {
            case DialogueEventType.EndDialogue:
                UIManager.Instance.DisplayDialogue(null); // Null entry closes the dialogue window
                break;
            case DialogueEventType.GiveItem:
                GameManager.Instance.AddItemToInventory(new InventoryItem(itemEventType, itemEventQuantity));
                break;
            default:
                Debug.Log("No event type defined!");
                break;
        }
    }
}

public enum DialogueEventType
{
    EndDialogue,
    GiveItem
}
