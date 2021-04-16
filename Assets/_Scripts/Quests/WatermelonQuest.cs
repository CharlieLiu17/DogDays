using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class WatermelonQuest : Quest //monobehavior "abstract class"
{
    [SerializeField]
    private Item watermelonItem;
    [SerializeField]
    private int amountNeeded;
    [SerializeField]
    private GameObject npc;
    private InventoryItem reward;
    private InventoryItem watermelonInvItem;
    private string dialogueEndName = "WatermelonQuestCompletion";
    private bool retrieved = false;

    public int rewardAmount;
    private void Start()
    {
        watermelonInvItem = new InventoryItem(watermelonItem, amountNeeded); // the desired item of the quest
        reward = new InventoryItem(dogTreats, rewardAmount);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    #region Quest Event Methods
    // These are different events we can use to trigger quest progress. 

    // Should be called infrequently and mainly used for resolving bugs.
    // For example, in a fetch quest we might use this to resolve the case where OnObtainItem() doesn't trigger when it should
    public override void OnUpdate()
    {
        OnObtainItem(null);
    }
    // Called when the quest is complete and runs some code for rewards, starting a new quest, etc.
    // For example: you collect all 3 bells, OnComplete is called and gives you 500 Gold and a new quest to collect 5 whistles
    public override void OnComplete()
    {
        GameManager.Instance.AddItemToInventory(reward);
        GameManager.Instance.RemoveQuestByID(id);
        UIManager.Instance.UpdateQuestsUI();
        if (GameManager.Instance.HasItem(dogTreats, 65))
        {
            GameManager.Instance.LoadNextScene(5, GameManager.Instance.finalTransforms);
        }
    }
    public override void OnObtainItem(InventoryItem item)
    {
        Predicate<InventoryItem> predicate = FindItem;
        if (Array.Find(GameManager.Instance.GetAllItemsAsArray(), predicate) != null)
        {
            description = "Give the slice back to Tilapia!";
            UIManager.Instance.UpdateQuestsUI();
            retrieved = true;
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
            GameManager.Instance.RemoveItemFromInventory(watermelonInvItem);
            OnComplete();
            retrieved = false;
        }
    }
    #endregion

    public bool FindItem(InventoryItem invItem)
    {
        return invItem.Equals(watermelonInvItem);
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        npc = GameObject.Find("Tilapia");
    }
}
