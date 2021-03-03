using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItem : MonoBehaviour
{
    public int space = 10;
    public static InventoryItem instance;
    void Awake() {
        instance = this;
    }
    public List<Item> items = new List<Item>();
    public int amount;

    public delegate void onItemChanged();
    public onItemChanged onItemChangedCallback;
    public void add(Item item) {
        if (items.Count >= 20) {
            Debug.Log(
                "not enough space ;-;"
            );
            return;
        }
        items.Add(item);
    }
    public void remove(Item item) {
        items.Remove(item);
    }
}
