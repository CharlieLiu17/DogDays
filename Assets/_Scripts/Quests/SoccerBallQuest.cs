using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class SoccerBallQuest : Quest
{
    [SerializeField]
    private Item ballItem;
    [SerializeField]
    private int amountNeeded;
    private InventoryItem ballInvItem;
    private InventoryItem reward;
    private bool retrieved = false;
    private string dialogueEndName;

    public int rewardAmount;
    [SerializeField]
    private GameObject npc;

    // Start is called before the first frame update
    private void Start()
    {
        ballInvItem = new InventoryItem(ballItem, amountNeeded); // the desired item of the quest
        reward = new InventoryItem(dogTreats, rewardAmount);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    #region Quest Event Methods
    // These are different events we can use to trigger quest progress. 

    // Should be called infrequently and mainly used for resolving bugs.
    // For example, in a fetch quest we might use this to resolve the case where OnObtainItem() doesn't trigger when it should
    public override void OnUpdate()
    {
        /*
        Predicate<InventoryItem> predicate = FindItem;
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
        Debug.Log("cmon man");
        if (Array.Find(GameManager.Instance.GetAllItemsAsArray(), predicate) != null)
        {
            Debug.Log("helloasdf");
            description = "Give the ball back to Cod!";
            UIManager.Instance.UpdateQuestsUI();
            retrieved = true;
            dialogueEndName = "SoccerBallQuestCompletion";
        }
    }
    public override void OnEnterRegion()
    {
        return;
    }
    public override void OnSpeakToNPC()
    {
        if (GameManager.Instance.getNpcEngaged().Equals(npc) && retrieved == true)
        {
            npc.GetComponent<DialogueHandler>().DialogueName = dialogueEndName;
            GameManager.Instance.RemoveItemFromInventory(ballInvItem);
            OnComplete();
            retrieved = false;
        }
    }
    public override void OnRemoveItem()
    {
        return;
    }
    #endregion

    public bool FindItem(InventoryItem invItem)
    {
        return invItem.Equals(ballInvItem);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        npc = GameObject.Find("Cod");
    }
}

