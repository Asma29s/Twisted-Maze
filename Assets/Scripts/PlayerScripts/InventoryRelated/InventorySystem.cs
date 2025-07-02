using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySystem : MonoBehaviour
{
    //player finds a key ==> click to pick up ==> it appears on the screen ==> it uses it to open door
    // once clicked it appears on the screen + stored in the list 
    // list should be checked if full once they click on the door to open 
    // 
    // inventory is all about keys for now: 
    // once interacting i will add the "item" to the arraylist
    // key will have an empty image >> set the orignal to false and the new to true 


    public static InventorySystem Instance; //singleton
    public List<keys> keysList;
    public int maxKeys = 3; //for exceptions
    public int keyCount; //for exceptions


    [Header("UI keys components")]
    [SerializeField] GameObject UImissingRed;
    [SerializeField] GameObject UImissingYellow;
    [SerializeField] GameObject UImissingBlue;

    [SerializeField] GameObject UIRedKey;
    [SerializeField] GameObject UIYellowKey;
    [SerializeField] GameObject UIBlueKey;



    private void Awake()
    {
        if (Instance == null) //imp for singeton
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); 
        }
    }
     
    public bool AddItem(keys keyN)
    { 
        // first check if full then add, add: true, full: false
        if (keysList.Count <= maxKeys)
        {
            keysList.Add(keyN);
            if (keyN.keyName == "RedKey")
            {
                UpdateUIDisactive(UImissingRed);
                UpdateUIActive(UIRedKey);
                keyCount++;
            }
            if (keyN.keyName == "YellowKey")
            {
                UpdateUIDisactive(UImissingYellow);
                UpdateUIActive(UIYellowKey);
                keyCount++;
            }
            if (keyN.keyName == "BlueKey")
            {
                UpdateUIDisactive(UImissingBlue);
                UpdateUIActive(UIBlueKey);
                keyCount++;
            }

            return true; // imp for test
        }
        return false; // inventory full
    }

    public bool CheckAllKeys()
    {
        return keysList.Count >= maxKeys;
        // if the array is full == return ture
        // if array is short on keys: 1) return false to not allow the player to open the door 

    }

    private void UpdateUIActive(GameObject key)
    {
        key.SetActive(true);
    }
    private void UpdateUIDisactive(GameObject key)
    {
        key.SetActive(false);
    }


}
