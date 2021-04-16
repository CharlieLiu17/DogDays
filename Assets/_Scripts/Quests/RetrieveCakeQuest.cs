using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[System.Serializable]
public class RetrieveCakeQuest : Quest //monobehavior "abstract class"
{
    [SerializeField]
    private Item cakeItem;
    [SerializeField]
    private int amountNeeded;
    private InventoryItem reward;
    private InventoryItem cakeInvItem;
    private bool haveItem = false;

    public int rewardAmount;
    private void Start()
    {
        cakeInvItem = new InventoryItem(cakeItem, amountNeeded); // the desired item of the quest
        reward = new InventoryItem(dogTreats, rewardAmount);
    }

    #region Quest Event Methods
    // These are different events we can use to trigger quest progress. 

    public override void OnStart()
    {
        if (GameManager.Instance.HasItem(cakeItem, 1))
        {
            StartCoroutine(DisplayDialogueAfterPause());
        }/**
        if (GameManager.Instance.HasItem(dogTreats, 65))
        {
            GameManager.Instance.LoadNextScene(5, GameManager.Instance.finalTransforms);
        }**/
    }

    private IEnumerator DisplayDialogueAfterPause()
    {
        yield return new WaitForSeconds(0.25f);
        DialogueHandler dh = GameManager.Instance.getCurrentDog().GetComponent<DialogueHandler>();
        dh.DialogueName = "ChocolateCake/ChocolateCakeQuestCompletion";
        UIManager.Instance.DialogueInitiationTextShown = false;
        dh.DisplayCurrentDialogue();
    }

    // Called when the quest is complete and runs some code for rewards, starting a new quest, etc.
    // For example: you collect all 3 bells, OnComplete is called and gives you 500 Gold and a new quest to collect 5 whistles
    public override void OnComplete()
    {
        GameManager.Instance.AddItemToInventory(reward);
        GameManager.Instance.RemoveQuestByID(id);
        UIManager.Instance.UpdateQuestsUI();
        GameObject.Find("Tuna").GetComponent<DialogueHandler>().setDialogueName("ChocolateCake/EarlyCompletion");
        if (GameManager.Instance.HasItem(dogTreats, 65))
        {
            GameManager.Instance.LoadNextScene(5, GameManager.Instance.finalTransforms);
        }
    }
    public override void OnObtainItem(InventoryItem item)
    {
        if (item != null && item.item.Equals(cakeItem))
        {
            DialogueHandler dh = GameManager.Instance.getCurrentDog().GetComponent<DialogueHandler>();
            dh.setDialogueName("ChocolateCake/ChocolateCakeQuestCompletion");
            dh.DisplayCurrentDialogue();
            UIManager.Instance.DialogueInitiationTextShown = false;
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
    public override void OnRemoveItem(InventoryItem item)
    {
        if (item != null && item.Equals(cakeInvItem))
        {
            OnComplete();
        }
    }
    #endregion
}
