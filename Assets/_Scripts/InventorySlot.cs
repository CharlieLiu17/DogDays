using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public Image icon;
    InventoryItem inventoryItem;

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
        icon.sprite = inventoryItem.item.sprite;
        icon.enabled = true;
        removeButton.interactable = true;
    }

    public void ClearSlot() {
        inventoryItem = null;
        icon.sprite = null;
        icon.enabled = false;
        removeButton.interactable = false;
    }

    public void OnRemoveButton() {
        GameManager.Instance.RemoveItemFromInventory(inventoryItem);
    }
}
