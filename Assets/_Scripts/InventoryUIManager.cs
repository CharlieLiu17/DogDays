
using UnityEngine;

public class InventoryUIManager : MonoBehaviour
{
    #region Singleton Code
    private static InventoryUIManager _instance;

    public static InventoryUIManager Instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Debug.LogWarning("Attempted to Instantiate multiple InventoryUIManagers in one scene!");
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    private void OnDestroy()
    {
        if (this == _instance) { _instance = null; }
    }
    #endregion

    [SerializeField]
    private Transform itemsParent;
    [SerializeField]
    private InventorySlot[] slots;
    
    void Start()
    {
        slots = itemsParent.GetComponentsInChildren<InventorySlot>();
    }

    // Fill inventory slots with all the items the player is carrying. Each slot manages their own appearance.
    public void UpdateInventoryUI() {
        if (slots == null)
        {
            slots = itemsParent.GetComponentsInChildren<InventorySlot>();
        }
        InventoryItem[] items = GameManager.Instance.GetAllItemsAsArray();
        Debug.Log("Updating Inventory UI");
        if (items.Length > slots.Length) // This shouldn't ever happen, but this is here in case it does
        {
            Debug.LogError("Player is carrying " + (items.Length - slots.Length) + " more items than their inventory capacity!");
        }

        // This for-loop setup looks weird I know, but it works nicely so forgive me pls
        int i;
        for (i = 0; i < slots.Length && i < items.Length; i++) // We fill out the slots with items, stopping when we run out of items or slots
        {
            if (slots[i] != null)
            {
                slots[i].AddItem(items[i]);
            }
        }
        for (; i < slots.Length; i++) // Make sure all other slots are empty
        {
            if (slots[i] != null)
            {
                slots[i].AddItem(items[i]);
            }
        }
    }
}
