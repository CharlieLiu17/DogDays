
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform itemsParent;
    InventoryItem inventory;

    InventorySlot[] slots;
    
    void Start()
    {
        inventory = InventoryItem.instance;
        inventory.onItemChangedCallback += UpdateUI;

        slots = itemsParent.GetComponentsInChildren<InventorySlot>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void UpdateUI() {
        Debug.Log("Updating UI");
        for (int i = 0; i < slots.Length; i++) {
            if (i < inventory.items.Count) {
                slots[i].AddItem(inventory.items[i]);
            } else {
                slots[i].clearSlot();
            }
        }
    
    }
}
