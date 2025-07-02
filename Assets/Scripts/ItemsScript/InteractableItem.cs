using UnityEngine;

public class InteractableItem : MonoBehaviour
{
    public keys keyName;
    public void PickUp()
    {
        
        if (gameObject.CompareTag("FlashLight"))
        {
            Debug.Log("Picked up: flashLight");
            FlashlightSystem.Instance.FlashlightActive();
            Destroy(gameObject);
        }
        if (gameObject.CompareTag("key"))
        {
            Debug.Log("Picked up: " + keyName.keyName);
            InventorySystem.Instance.AddItem(keyName);
            Destroy(gameObject);
        }
    }

}
