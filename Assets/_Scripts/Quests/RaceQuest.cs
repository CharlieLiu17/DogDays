using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[System.Serializable]
public class RaceQuest : Quest
{
    // This is the actual dog treats that appear in the inventory.
    private InventoryItem reward;
    public int rewardAmount;
    // because id is a public instance variable in Quest, it is inherited and set in the inspector
    // this is really weird

    public GameObject pesto;

    // Does this not need to be instantiated?
    public treeCollideScript treeScript;


    // Start is called before the first frame update
    private void Start()
    {
        reward = new InventoryItem(dogTreats, rewardAmount);   
    }

    #region Quest Event Methods
    // These are different events we can use to trigger quest progress. 

    // Should be called infrequently and mainly used for resolving bugs.
    // For example, in a fetch quest we might use this to resolve the case where OnObtainItem() doesn't trigger when it should
    public override void OnUpdate()
    {
        // this could get really shaky if anything else uses OnUpdate, but they're not so lmfao
        pesto.GetComponent<PestoMovement>().questIsActive = true;

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
        return;
    }
    public override void OnEnterRegion()
    {
        // This is called once either Pesto or main character enter tree and quest is active


        if (treeScript.GetWhoEntered().Equals(pesto)) 
        {
            print("pesto won you should be sad");
        } else
        {
            this.OnComplete();
        }
    }
    public override void OnSpeakToNPC()
    {
        return;
    }
    #endregion

}