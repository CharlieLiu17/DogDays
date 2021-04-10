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

    #region Controls
    public class Controls
    {
        public static KeyCode Up;
        public static KeyCode Right;
        public static KeyCode Down;
        public static KeyCode Left;
        public static KeyCode Sprint;
        public static KeyCode Interract;
        public static KeyCode OpenMenu;
        public static KeyCode OpenInventory;
    }

    public void GetControlsFromPlayerPrefs()
    {
        Controls.Up = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("up", "W"));
        Controls.Right = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("right", "D"));
        Controls.Down = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("down", "S"));
        Controls.Left = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("left", "S"));
        Controls.Sprint = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("sprint", "LeftShift"));
        Controls.Interract = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("interract", "E"));
        Controls.OpenMenu = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("openMenu", "Escape"));
        Controls.OpenInventory = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("openInventory", "Tab"));
    }
    #endregion

    private void Start()
    {
        GetControlsFromPlayerPrefs();
    }

    private void Update()
    {

    }

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
