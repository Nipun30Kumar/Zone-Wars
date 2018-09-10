using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour {

    public string itemName;
    public string coinType;
    private bool itemSelected = false;
    private Sprite itemImage;

    InventoryPanelManager inventoryManager;

    void Start()
    {
        inventoryManager = FindObjectOfType<InventoryPanelManager>();
        itemName = this.gameObject.name;
        itemImage = GameObject.Find(itemName + "/Coin Image").GetComponent<Image>().sprite;
        if (inventoryManager.playerID == 1)
        {
            if (itemName == "Simple Coin")
            {
                itemName = "Simple Coin_1";
            }
            else if (itemName == "Simple Coin Large")
            {
                itemName = "Simple Coin Large_1";
            }
        }
        else if (inventoryManager.playerID == 2)
        {
            if (itemName == "Simple Coin")
            {
                itemName = "Simple Coin_2";
            }
            else if (itemName == "Simple Coin Large")
            {
                itemName = "Simple Coin Large_2";
            }
        }
    }
	
    public void OnClickItem()
    {        
        Debug.Log("Button " + itemName + " clicked " + itemImage);
        if (inventoryManager.playerDeck.Contains(itemName))
        {
            Debug.Log("Inventory already contains the selected coin !");
        }
        else
        {
            inventoryManager.CheckConstraints(itemName, coinType, itemImage);
        }
    }

}
