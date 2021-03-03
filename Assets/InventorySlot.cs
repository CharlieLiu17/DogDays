using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public Image icon;
    Item item1;

    public Button removeButton;
    public Image getIcon() {
        return this.icon;
    }

    public void AddItem(Item newItem) {
        item1 = newItem;
        icon.sprite = item1.sprite;
        icon.enabled = true;
        removeButton.interactable = true;

    }

    public void clearSlot() {
        item1 = null;
        icon.sprite = null;
        icon.enabled = false;
        removeButton.interactable = false;
    }

    public void onRemoveButton() {
        InventoryItem.instance.remove(item1);
    }
}
