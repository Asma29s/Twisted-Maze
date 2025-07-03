using UnityEngine;

public class WinDoor : MonoBehaviour
{
    public static WinDoor Instance; //singleton
    public InventorySystem inventorySystem;
    public WinMenu winMenuUI;
    public GameObject failTXT; // "you need to collect all keys"

    public GameObject winDoor;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        failTXT.SetActive(false);
    }

    public void PlayerWonCheck()
    {
        if (inventorySystem.CheckAllKeys())
        {
            Debug.Log("YOU WON ! ! ! !");
            winMenuUI.PlayerWon();
        }
        else
        {
            Debug.Log("YOU ! ! ! ! didnt win :< ");
            failTXT.SetActive(true);
        }
    }
}
