using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[System.Serializable]
public class WatermelonQuest : Quest //monobehavior "abstract class"
{
    [SerializeField]
    private Item watermelonItem;
    [SerializeField]
    private int amountNeeded;
    private InventoryItem reward;
    private InventoryItem watermelonInvItem;
    private bool haveItem = false;

    public int rewardAmount;
    private void Start()
    {
        watermelonInvItem = new InventoryItem(watermelonItem, amountNeeded); // the desired item of the quest
        reward = new InventoryItem(dogTreats, rewardAmount);
    }

    #region Quest Event Methods
    // These are different events we can use to trigger quest progress. 

    // Should be called infrequently and mainly used for resolving bugs.
    // For example, in a fetch quest we might use this to resolve the case where OnObtainItem() doesn't trigger when it should
    public override void OnUpdate()
    {
        /*Predicate<InventoryItem> predicate = FindItem;
        if (Array.Find(GameManager.Instance.GetAllItemsAsArray(), predicate) != null)
        {
            OnComplete();
        } */

    }
    // Called when the quest is complete and runs some code for rewards, starting a new quest, etc.
    // For example: you collect all 3 bells, OnComplete is called and gives you 500 Gold and a new quest to collect 5 whistles
    public override void OnComplete()
    {
        GameManager.Instance.AddItemToInventory(reward);
        GameManager.Instance.RemoveQuestByID(id);
        UIManager.Instance.UpdateQuestsUI();
    }
    public override void OnObtainItem()
    {
        Predicate<InventoryItem> predicate = FindItem;
        if (Array.Find(GameManager.Instance.GetAllItemsAsArray(), predicate) != null)
        {
            DialogueHandler dh = GameManager.Instance.getCurrentDog().GetComponent<DialogueHandler>();
            dh.DialogueName = "ChocolateCakeQuestCompletion";
            dh.DisplayCurrentDialogue();
            UIManager.Instance.DialogueInitiationTextShown = false;
            haveItem = true;
            Debug.Log("Great Job!");
        }
    }
    public override void OnEnterRegion()
    {
        return;
    }
    public override void OnSpeakToNPC()
    {
        return;
    }
    public override void OnRemoveItem()
    {
        Predicate<InventoryItem> predicate = FindItem;
        if (haveItem && Array.Find(GameManager.Instance.GetAllItemsAsArray(), predicate) == null)
        {
            OnComplete();
        }
    }
    #endregion

    public bool FindItem(InventoryItem invItem)
    {
        return invItem.Equals(cakeInvItem);
    }
}
