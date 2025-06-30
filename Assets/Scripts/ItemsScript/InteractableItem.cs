using UnityEngine;

public class InteractableItem : MonoBehaviour
{
    public string itemName;

    public void PickUp()
    {
        Debug.Log("Picked up: " + itemName);
        // InventoryManager.Instance.AddItem(itemName);
        Destroy(gameObject);
    }
}
