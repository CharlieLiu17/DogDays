using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Quests/Quest", order = 2)]
public class Quest : ScriptableObject
{
    public int id;
    public string displayName;
    public string internalName;
    public string description;
    public Sprite sprite;
    public QuestTypes type; // See the bottom of this file for details

    #region Quest Event Methods
    // These are different events we can use to trigger quest progress. 

    // Should be called infrequently and mainly used for resolving bugs.
    // For example, in a fetch quest we might use this to resolve the case where OnObtainItem() doesn't trigger when it should
    public virtual void OnUpdate()
    {
        
    }
    // Called when the quest is complete and runs some code for rewards, starting a new quest, etc.
    // For example: you collect all 3 bells, OnComplete is called and gives you 500 Gold and a new quest to collect 5 whistles
    public virtual void OnComplete()
    {

    }
    public virtual void OnObtainItem()
    {

    }
    public virtual void OnEnterRegion()
    {

    }
    public virtual void OnSpeakToNPC()
    {

    }
    #endregion
}
// These are numbered so they correspond to QuestTypes and can be matched to them with a cast
public enum QuestEvent
{
    UPDATE = -2,
    COMPLETE = -1,
    OBTAIN_ITEM = 0,
    ENTER_REGION = 1,
    SPEAK_TO_NPC = 2
}
// This are used improve efficiency by only calling certain action calls when the player has at least one quest of the corresponding type
public enum QuestTypes
{
    OBTAIN = 0,
    ENTER = 1,
    SPEAK = 2
}
