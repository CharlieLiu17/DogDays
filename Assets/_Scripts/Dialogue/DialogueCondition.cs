using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// It might be better to make each condition type its own subclass, but that feels unneeded with the amount of conditions I can currently think of.
public class DialogueCondition
{
    public DialogueConditionType type;

    public Item itemConditionType;
    public int itemConditionQuantity;

    public DialogueCondition(DialogueConditionType type, Item itemConditionType, int itemConditionQuantity)
    {
        this.type = type;
        this.itemConditionType = itemConditionType;
        this.itemConditionQuantity = itemConditionQuantity;
    }

    public bool GetValue()
    {
        switch (type)
        {
            case DialogueConditionType.HasItem:
                return GameManager.Instance.HasItem(itemConditionType, itemConditionQuantity);
            default:
                return true; // Return true if the condition is invalid
        }
    }
}

public enum DialogueConditionType
{
    HasItem
}
