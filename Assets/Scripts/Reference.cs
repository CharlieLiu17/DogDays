using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Singelton component containing game data such as lists of quests, items, etc.
// Any data you want to be globally available should probably go here.
public class Reference : MonoBehaviour
{
    // This code allows us to access Reference's methods and data by using Reference.Instance.<method/data name> from anywhere
    #region Singleton Code
    private static Reference _instance;

    public static Reference Instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Debug.LogWarning("Attempted to Instantiate multiple Reference objects in one scene!");
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
        InitializeDictionaries();
    }

    private void OnDestroy()
    {
        if (this == _instance) { _instance = null; }
    }
    #endregion

    // List all of quests in the game. Set from the editor
    [SerializeField]
    private Quest[] quests;
    // List of all items in the game. Set from the editor
    [SerializeField]
    private Item[] items;

    #region Dictionaries
    // Used for getting items and quests by their name or ID
    private Dictionary<int, Item> itemsByID = null;
    private Dictionary<string, Item> itemsByName = null;
    private Dictionary<int, Quest> questsByID = null;
    private Dictionary<string, Quest> questsByName = null;

    // This is called in awake
    private void InitializeDictionaries()
    {
        itemsByID = new Dictionary<int, Item>();
        foreach (Item item in items) {
            itemsByID.Add(item.id, item);
            itemsByName.Add(item.internalName, item);
        }
        questsByID = new Dictionary<int, Quest>();
        foreach (Quest quest in quests)
        {
            questsByID.Add(quest.id, quest);
            questsByName.Add(quest.internalName, quest);
        }
    }
    #endregion
}
