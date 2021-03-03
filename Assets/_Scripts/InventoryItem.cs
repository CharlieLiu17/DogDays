using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Container class for an item stored in the player's inventory.
public class InventoryItem
{
    public Item item;
    public int amount;

    public InventoryItem(Item item, int amount)
    {
        this.item = item;
        this.amount = amount;
    }
}