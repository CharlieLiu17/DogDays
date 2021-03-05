using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    [SerializeField]
    private Image icon;

    [SerializeField]
    private Sprite emptySprite; // Sprite to show when the slot is empty
    private InventoryItem inventoryItem;

    public Button removeButton;
    public Image getIcon() {
        return this.icon;
    }

    public void AddItem(InventoryItem inventoryItem) {
        if (inventoryItem == null)
        {
            Debug.LogError("Attempted to add null item to player inventory");
            return;
        }
        this.inventoryItem = inventoryItem;
        removeButton.interactable = true;
        icon.sprite = inventoryItem.item.sprite;
    }

    public void ClearSlot() {
        inventoryItem = null;
        icon.sprite = null;
        icon.sprite = emptySprite;
        removeButton.interactable = false;
    }

    public void OnRemoveButton() {
        GameManager.Instance.RemoveItemFromInventory(inventoryItem);
    }
}
