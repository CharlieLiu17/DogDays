using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[System.Serializable]
public class PhoneQuest : Quest
{
    [SerializeField]
    private Item phoneItem;
    [SerializeField]
    private int amountNeeded;
    private InventoryItem phoneInvItem;

    // Start is called before the first frame update
    private void Start()
    {
        phoneInvItem = new InventoryItem(phoneItem, amountNeeded); // the desired item of the quest
    }

    #region Quest Event Methods
    // These are different events we can use to trigger quest progress. 

    // Should be called infrequently and mainly used for resolving bugs.
    // For example, in a fetch quest we might use this to resolve the case where OnObtainItem() doesn't trigger when it should
    public override void OnUpdate()
    {
        Predicate<InventoryItem> predicate = FindItem;
        if (Array.Find(GameManager.Instance.GetAllItemsAsArray(), predicate) != null)
        {
            OnComplete();
        }

    }
    // Called when the quest is complete and runs some code for rewards, starting a new quest, etc.
    // For example: you collect all 3 bells, OnComplete is called and gives you 500 Gold and a new quest to collect 5 whistles
    public override void OnComplete()
    {
        Debug.Log("Great Job on getting the phone!");
    }
    public override void OnObtainItem(InventoryItem item)
    {
        OnComplete();
    }
    public override void OnEnterRegion()
    {
        return;
    }
    public override void OnSpeakToNPC()
    {
        return;
    }
    #endregion

    public bool FindItem(InventoryItem invItem)
    {
        return invItem.Equals(phoneInvItem);
    }
}