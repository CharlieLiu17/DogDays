using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

// Singelton component containing game data such as lists of quests, items, etc.
// Any data that doesn't change during play and you want to be globally available should probably go here.
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
        Initialize();
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
    // Used for getting items, quests, and dialogue entries by their name or ID
    private Dictionary<int, Item> itemsByID = null;
    private Dictionary<string, Item> itemsByName = null;
    private Dictionary<int, Quest> questsByID = null;
    private Dictionary<string, Quest> questsByName = null;

    // This is called in awake
    private void Initialize()
    {
        itemsByID = new Dictionary<int, Item>();
        itemsByName = new Dictionary<string, Item>();
        if (items != null) // These if statements prevent NREs when we have no items/quests set in the inspector
        {
            foreach (Item item in items)
            {
                if (item != null)
                {
                    itemsByID.Add(item.id, item);
                    itemsByName.Add(item.internalName, item);
                }
            }
        }
        questsByID = new Dictionary<int, Quest>();
        questsByName = new Dictionary<string, Quest>();
        if (quests != null)
        {
            foreach (Quest quest in quests)
            {
                if (quest != null && !questsByID.ContainsKey(quest.id)) // Don't add quest if we already have a matching ID
                {
                    questsByID.Add(quest.id, quest);
                    questsByName.Add(quest.internalName, quest);
                }
            }
        }
    }
    #endregion

    #region Quests
    public Quest GetQuestByName(string internalName)
    {
        if (questsByName.ContainsKey(name))
        {
            return questsByName[name];
        }
        else
        {
            Debug.LogError("Attempted to get Quest by invalid name");
            return null;
        }
    }
    public Quest GetQuestByID(int id)
    {
        if (questsByID.ContainsKey(id))
        {
            return questsByID[id];
        }
        else
        {
            Debug.LogError("Attempted to get Quest by invalid ID");
            return null;
        }
    }
    #endregion

    #region Items
    public Item GetItemByName(string internalName)
    {
        if (itemsByName.ContainsKey(name))
        {
            return itemsByName[name];
        }
        else
        {
            Debug.LogError("Attempted to get Item by invalid name");
            return null;
        }
    }
    public Item GetItemByID(int id)
    {
        if (itemsByID.ContainsKey(id))
        {
            return itemsByID[id];
        }
        else
        {
            Debug.LogError("Attempted to get Item by invalid ID");
            return null;
        }
    }
    #endregion
}
