using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RemoveDeckSlot : MonoBehaviour {

    public int slotID;

    InventoryPanelManager inventoryManager;

    void Start()
    {
        inventoryManager = FindObjectOfType<InventoryPanelManager>();
    }

	public void OnClickClose()
    {
        if(inventoryManager.deckCards[slotID - 1].GetComponent<Image>().sprite == null)
        {
            return;
        }
        if((inventoryManager.playerDeck[slotID - 1] == "Monk Coin") || (inventoryManager.playerDeck[slotID - 1] == "Bomb Coin") && inventoryManager.specialCoinCount >= 0)
        {
            inventoryManager.specialCoinCount--;
        }
        else if ((inventoryManager.playerDeck[slotID - 1] == "Glue Coin") || (inventoryManager.playerDeck[slotID - 1] == "Grease Coin") && inventoryManager.powerupCount >= 0)
        {
            inventoryManager.powerupCount--;
        }
        else if(inventoryManager.simpleCoinCount >= 0)
        {
            inventoryManager.simpleCoinCount--;
        }
        else
        {
            Debug.Log("The coin count lower limit has reached");
        }
        inventoryManager.deckCards[slotID - 1].GetComponent<Image>().sprite = inventoryManager.emptySlotImage;
        inventoryManager.playerDeck[slotID - 1] = null;
        
    }
}
