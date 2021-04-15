using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Represents an item in the game world than can be collected.
public class ItemEntity : MonoBehaviour
{
    [SerializeField]
    private Item item;

    [SerializeField]
    private int quantity;

    // Start is called before the first frame update
    void Start()
    {
        if (item == null)
        {
            Debug.LogError("Item was null! Did you forget to set it in the inspector?");
        }
        if (quantity <= 0)
        {
            Debug.LogWarning("Quantity was invalid (<= 0), defaulting to 1");
            quantity = 1;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player" && Input.GetKeyDown(KeyCode.E))
        {
            GameManager.Instance.AddItemToInventory(new InventoryItem(this.item, this.quantity));
            UIManager.Instance.ItemPickupTextShown = false;
            Destroy(gameObject);
        }
    }

    // If both dogs walk into collection range and one exits, the text will no longer appear. I'm not quite sure how to solve this yet.
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player") // Both the dogs have the Player tag
        {
            UIManager.Instance.ItemPickupTextShown = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player") // Both the dogs have the Player tag
        {
            UIManager.Instance.ItemPickupTextShown = false;
        }
    }
}
