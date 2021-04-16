using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;

// Singelton component for managing the game.
// Any methods you want to be globally available should probably go here along with any data that changes during play.
public class GameManager : MonoBehaviour
{
    // This is exposed to the editor for debugging, but we shouldn't add quests in this way while in Play Mode
    [SerializeField]
    private List<Quest> activeQuests = null;
    [SerializeField]
    private List<InventoryItem> inventory = null;
    [SerializeField]
    private Animator transition;
    [SerializeField]
    private int transitionTime;
    [SerializeField]
    private GameObject[] dontDestroy; //THE REFERENCES TO THE DOGS
    private GameObject npcEngaged;
    private bool inDoor;
    public Transform[] finalTransforms;
    public bool finale;
    public Item dogTreats;


    public CinemachineFreeLook freeLookScript;
    [SerializeField]
    private Transform[] dogSpawnTransforms = new Transform[2];
    // This code allows us to access GameManager's methods and data by using GameManager.Instance.<method/data name> from anywhere
    #region Singleton Code
    private static GameManager _instance;

    public static GameManager Instance { get { return _instance; } }

    private void Awake()
    {

        if (_instance != null && _instance != this)
        {
            Debug.LogWarning("Attempted to Instantiate multiple GameManagers in one scene!");
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
        DontDestroyOnLoad(gameObject);
        foreach (GameObject anObject in dontDestroy)
        {
            if (GameObject.Find(anObject.name) == null)
            {
                anObject.SetActive(true);
            }
        }

    }

    private void OnDestroy()
    {
        if (this == _instance) { _instance = null; }
    }
    #endregion


    // Allows us to check if the player has a quest of the given type
    private Dictionary<QuestTypes, bool> hasQuestType = null;

    // Make sure this is = to the number of actual UI inventory slots
    public int InventoryCapacity { get; set; }

    private bool cursorLocked;
    public bool CursorLocked {
        get { return cursorLocked; }
        set
        {
            cursorLocked = value;
            if (value)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            } else
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
    }

    public Dogs currentDog;

    void Start()
    {
        if (inventory == null)
        {
            inventory = new List<InventoryItem>();
        }
        // Initialize dictionary with all false since the player starts with no quests
        // If we want savegames we will need to change this
        hasQuestType = new Dictionary<QuestTypes, bool>
        {
            { QuestTypes.ENTER, false },
            { QuestTypes.OBTAIN, false },
            { QuestTypes.SPEAK, false }
        };
        SceneManager.sceneLoaded += OnSceneLoaded;

        // Testing inventory system
        //AddItemToInventory(Reference.Instance.GetItemByID(1), 1);
        //AddItemToInventory(Reference.Instance.GetItemByID(0), 3);
        //AddItemToInventory(Reference.Instance.GetItemByID(2), 1);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex == 5)
        {
            DialogueHandler dh = GameManager.Instance.getCurrentDog().GetComponent<DialogueHandler>();
            dh.setDialogueName("Final/1");
            dh.DisplayCurrentDialogue();
            UIManager.Instance.DialogueInitiationTextShown = false;
        }
    }

    #region Quests
    public void AddQuest(Quest quest)
    {
        if (!activeQuests.Contains(quest)) // Don't allow duplicates
        {
            activeQuests.Add(quest);
            UIManager.Instance.UpdateQuestsUI();
            TriggerQuestEvent(QuestEvent.START);
        }
    }
    public void RemoveQuestByName(string name)
    {
        activeQuests.Remove(activeQuests.Find(x => x.internalName == name));
        UpdateHasQuestType();
        UIManager.Instance.UpdateQuestsUI();
    }
    public void RemoveQuestByID(int id)
    {
        activeQuests.Remove(activeQuests.Find(x => x.id == id));
        UpdateHasQuestType();
        UIManager.Instance.UpdateQuestsUI();
    }
    private void UpdateHasQuestType()
    {
        // Reset all to false, then set to true if we find a quest of that type
        hasQuestType = new Dictionary<QuestTypes, bool>
        {
            { QuestTypes.ENTER, false },
            { QuestTypes.OBTAIN, false },
            { QuestTypes.SPEAK, false }
        };
        foreach (Quest quest in activeQuests)
        {
            switch (quest.type)
            {
                case QuestTypes.ENTER:
                    hasQuestType[QuestTypes.ENTER] = true;
                    break;
                case QuestTypes.OBTAIN:
                    hasQuestType[QuestTypes.OBTAIN] = true;
                    break;
                case QuestTypes.SPEAK:
                    hasQuestType[QuestTypes.SPEAK] = true;
                    break;
            }
        }
    }
    public void TriggerQuestEvent(QuestEvent qe, InventoryItem item = null)
    {
        try
        {
            foreach (Quest quest in activeQuests)
            {
                switch (qe)
                {
                    case QuestEvent.COMPLETE:
                        quest.OnComplete();
                        break;
                    case QuestEvent.UPDATE:
                        quest.OnUpdate();
                        break;
                    case QuestEvent.ENTER_REGION:
                        quest.OnEnterRegion();
                        break;
                    case QuestEvent.OBTAIN_ITEM:
                        quest.OnObtainItem(item);
                        break;
                    case QuestEvent.SPEAK_TO_NPC:
                        quest.OnSpeakToNPC();
                        break;
                    case QuestEvent.REMOVE_ITEM:
                        quest.OnRemoveItem(item);
                        break;
                    case QuestEvent.START:
                        quest.OnStart();
                        break;
                    default:
                        break;
                }
            }
        }
        catch (System.InvalidOperationException)
        {
            //Debug.LogError("Caught InvalidOperationException");
        }
    }

    public GameObject getNpcEngaged()
    {
        return npcEngaged;
    }

    public void setNpcEngaged(GameObject npc)
    {
        npcEngaged = npc;
    }

    public List<Quest> getActiveQuests()
    {
        return activeQuests;
    }
    #endregion

    #region Inventory
    public void AddItemToInventory(Item item, int amount)
    {
        if (inventory.Find(invItem => invItem.item == item) != null) // Handle multiple of the same item
        {
            inventory.Find(invItem => invItem.item == item).amount += amount;
        }
        else
        {
            inventory.Add(new InventoryItem(item, amount));
        }
        // Right now we're updating the display every time it changes. Once we have inventory be opened using a key we should call this method
        // then and only then. That goes for all the calls to UpdateInventoryUI you see in this file.
        UIManager.Instance.UpdateInventoryUI();
        TriggerQuestEvent(QuestEvent.OBTAIN_ITEM, new InventoryItem(item, amount));
    }
    public void AddItemToInventory(InventoryItem inventoryItem)
    {
        // Handle multiple of the same item
        // We use == here because item refers to a scriptable object of which there is only one instance
        if (inventory.Find(invItem => invItem.item == inventoryItem.item) != null)
        {
            inventory.Find(invItem => invItem.item == inventoryItem.item).amount += inventoryItem.amount;
        }
        else
        {
            inventory.Add(inventoryItem);
        }
        UIManager.Instance.UpdateInventoryUI();
        TriggerQuestEvent(QuestEvent.OBTAIN_ITEM, inventoryItem);
    }
    // Amount defaults to 1 because removing 0 doesn't make sense.
    public void RemoveItemFromInventory(Item item, int amount = 1)
    {
        InventoryItem correspondingEntry = inventory.Find(i => i.item == item);

        // Player doesn't have the item
        if (correspondingEntry == null)
        {
            return;
        }
        else
        {
            TriggerQuestEvent(QuestEvent.REMOVE_ITEM, correspondingEntry);
            correspondingEntry.amount -= amount;
            // If we have a quantity of <= to 0, we take the item entry out
            if (correspondingEntry.amount <= 0)
            {
                inventory.Remove(correspondingEntry);
            }
        }
        UIManager.Instance.UpdateInventoryUI();
    }
    public void RemoveItemFromInventory(InventoryItem inventoryItem)
    {
        if (inventoryItem == null)
        {
            Debug.LogError("Attempted to remove null item from inventory");
            return;
        }
        InventoryItem correspondingEntry = inventory.Find(invItem => invItem.item == inventoryItem.item);

        // Player doesn't have the item
        if (correspondingEntry == null)
        {
            return;
        }
        else
        {
            TriggerQuestEvent(QuestEvent.REMOVE_ITEM, correspondingEntry);
            inventory.Remove(correspondingEntry);
        }
        UIManager.Instance.UpdateInventoryUI();
        foreach (InventoryItem item in inventory)
        {
            Debug.Log(item.ToString());
        }
    }
    public bool HasItem(Item item, int amount)
    {
        InventoryItem correspondingEntry = inventory.Find(invItem => invItem.item == item);
        if (correspondingEntry == null || correspondingEntry.amount < amount)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    public InventoryItem[] GetAllItemsAsArray()
    {
        return inventory.ToArray();
    }
    public List<InventoryItem> GetAllItemsAsList()
    {
        return inventory;
    }
    #endregion

    #region SceneTransition

    public void LoadNextScene(int buildIndex, Transform[] nextTransforms)
    {
        Debug.Log(nextTransforms[0].position);
        dogSpawnTransforms[0].position = nextTransforms[0].position;
        dogSpawnTransforms[0].rotation = nextTransforms[0].rotation;
        dogSpawnTransforms[1].position = nextTransforms[1].position;
        dogSpawnTransforms[1].rotation = nextTransforms[1].rotation;
        StartCoroutine(LoadScene(buildIndex, nextTransforms));
    }

    IEnumerator LoadScene(int buildIndex, Transform[] nextTransforms)
    {
        transition.SetBool("Transition", true);

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(buildIndex);


        GameObject kobe = dontDestroy[0];
        GameObject kali = dontDestroy[1];

        kobe.transform.position = dogSpawnTransforms[0].position; //index 0 always kobe
        kali.transform.rotation = dogSpawnTransforms[0].rotation;
        kali.transform.position = dogSpawnTransforms[1].position; //index 1 always kali
        kali.transform.rotation = dogSpawnTransforms[1].rotation;

        transition.SetBool("Transition", false);
    }

    public void Restart()
    {
        GameObject kobe = dontDestroy[0];
        GameObject kali = dontDestroy[1];
        Debug.Log(dogSpawnTransforms[0].position);
        kobe.transform.position = dogSpawnTransforms[0].position;
        kobe.transform.rotation = dogSpawnTransforms[0].rotation;
        kali.transform.position = dogSpawnTransforms[1].position;
        kali.transform.rotation = dogSpawnTransforms[1].rotation;
    }
    public GameObject getKobe()
    {
        return dontDestroy[0];
    }

    public GameObject getKali()
    {
        return dontDestroy[1];
    }
    public GameObject getCurrentDog()
    {
        switch (currentDog)
        {
            case Dogs.Kobe:
                return dontDestroy[0];
            case Dogs.Kali:
                return dontDestroy[1];
            default:
                return null;

        }
    }

    

    #endregion

    public bool getInDoor()
    {
        return inDoor;
    }
    public void setInDoor(bool value)
    {
        inDoor = value;
    }
}
public enum Dogs { 
    Kobe, 
    Kali 
}

