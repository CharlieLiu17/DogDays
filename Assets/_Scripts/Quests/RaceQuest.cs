using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class RaceQuest : Quest
{
    // This is the actual dog treats that appear in the inventory.
    private InventoryItem reward;
    public int rewardAmount;
    public int hasWon = 0;
    public Transform[] finalTransforms;

    public String pestoWinDialogue = "PestoWinDialogue";
    public String pestoLoseDialogue = "PestoLoseDialogue";
    // because id is a public instance variable in Quest, it is inherited and set in the inspector
    // this is really weird


    public GameObject pesto;
    // Does this not need to be instantiated?
    public treeCollideScript treeScript;


    // Start is called before the first frame update
    private void Start()
    {
        reward = new InventoryItem(dogTreats, rewardAmount);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    #region Quest Event Methods
    // These are different events we can use to trigger quest progress. 

    // Should be called infrequently and mainly used for resolving bugs.
    // For example, in a fetch quest we might use this to resolve the case where OnObtainItem() doesn't trigger when it should
    public override void OnUpdate()
    {
        // this could get really shaky if anything else uses OnUpdate, but they're not so lmfao
        if (pesto != null)
        {
            pesto.GetComponent<PestoMovement>().questIsActive = true;
        }
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
        return;
    }
    public override void OnEnterRegion()
    {
        // This is called once either Pesto or main character enter tree and quest is active

        //haswon = 1 means pesto won.
        Debug.Log(treeScript.GetWhoEntered());
        if (GameObject.ReferenceEquals(pesto, treeScript.GetWhoEntered())) 
        {
            pesto.GetComponent<PestoMovement>().questIsActive = false;
            hasWon = 1;
        } else
        {
            pesto.GetComponent<PestoMovement>().questIsActive = false;
            pesto.GetComponent<PestoMovement>().crying = true;
            hasWon = 2;
        }
    }
    public override void OnSpeakToNPC()
    {
        if (GameManager.Instance.getNpcEngaged().Equals(pesto) && hasWon == 2)
        {
            Debug.Log("wow341");
            pesto.GetComponent<DialogueHandler>().DialogueName = pestoLoseDialogue;
            OnComplete();
        } else if (GameManager.Instance.getNpcEngaged().Equals(pesto) && hasWon == 1)
        {
            Debug.Log("hello234");
            pesto.GetComponent<DialogueHandler>().DialogueName = pestoWinDialogue;
            OnComplete();
        }
    }
    #endregion
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        pesto = GameObject.Find("Pesto Please Don't Leave");
        if (GameObject.Find("QuestTree") != null)
        {
            treeScript = GameObject.Find("Quest Tree").GetComponent<treeCollideScript>();
        }
    }
}