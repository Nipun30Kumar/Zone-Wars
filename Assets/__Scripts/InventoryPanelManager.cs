using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class InventoryPanelManager : MonoBehaviour {

    public List<GameObject> deckCards;      // FINAL DECK.
    public List<string> playerDeck;         // THE ACTUAL LIST FOR FORWARDING DATA TO THE GAMEPLAY CONTROLLER.
    public Button confirmButton;
    public TextMeshProUGUI headerText;
    public Sprite emptySlotImage;

    public int simpleCoinCount;
    private int maxSimpleCoins = 3;
    public int specialCoinCount;
    private int maxSpecialCoins = 2;
    public int powerupCount;
    private int maxPowerupCount = 1;
    public int playerID;

    private bool selectionMade = false;
    MainMenuController menuController;

    void Awake()
    {
        playerID = 1;
        menuController = FindObjectOfType<MainMenuController>();
    }

	void Start () {
        confirmButton.gameObject.SetActive(false);
        selectionMade = false;
        simpleCoinCount = 0;
        specialCoinCount = 0;
        powerupCount = 0;
	}

    void Update()
    {
        if (selectionMade)
        {
            confirmButton.gameObject.SetActive(true);
        }
    }

    public void CheckConstraints(string itemName, string coinType, Sprite itemImage)
    {
        if (coinType == "Simple Coin" && simpleCoinCount < maxSimpleCoins)
        {
            //Debug.Log("Coin name : " + itemName + " Coin Type : " + coinType + "Coin Image :" + itemImage);
            simpleCoinCount++;
            UpdateDeck(itemName, itemImage);
        }
        else if(coinType == "Special Coin" && specialCoinCount < maxSpecialCoins)
        {
            //Debug.Log("Coin name : " + itemName + " Coin Type : " + coinType + "Coin Image :" + itemImage);
            specialCoinCount++;
            UpdateDeck(itemName, itemImage);
        }
        else if (coinType == "Powerup" && powerupCount < maxPowerupCount)
        {
            Debug.Log("Coin name : " + itemName + " Coin Type : " + coinType + "Coin Image :" + itemImage);
            powerupCount++;
            UpdateDeck(itemName, itemImage);
        }
        else if((simpleCoinCount + specialCoinCount + powerupCount) == (maxSimpleCoins + maxSpecialCoins + maxPowerupCount))
        {
            Debug.Log("Deck can only contain 5 cards");
        }
    }

    void UpdateDeck(string itemName, Sprite itemImage)
    {
        for(int i = 0; i < deckCards.Count; i++)
        {   
            if (deckCards[i].GetComponent<Image>().sprite == emptySlotImage)
            {           
                deckCards[i].GetComponent<Image>().sprite = itemImage;
                playerDeck[i] = itemName;
                if (i == 4 && deckCards[i].GetComponent<Image>().sprite != null)
                {
                    selectionMade = true;
                    Debug.Log("The deck is full. Remove a card from the deck first to add another.");
                }
                break;
            }
        }
    }

    public void ConfirmDeck()
    {
        if(playerID == 1)
        {
            for(int i = 0; i < deckCards.Count; i++)
            {
                if(Resources.Load<GameObject>("_Prefabs/" + playerDeck[i]) == null)
                {
                    Debug.Log("Invalid Item");
                }
                GameplayController.spawnObjects_PL1[i] = Resources.Load<GameObject>("_Prefabs/" + playerDeck[i]);
            }
            headerText.text = "2";
            playerID = 2;
        }
        else if(playerID == 2)
        {
            for (int i = 0; i < GameplayController.spawnObjects_PL2.Length; i++)
            {
                if (Resources.Load<GameObject>("_Prefabs/" + playerDeck[i]) == null)
                {
                    Debug.Log("Invalid Item");
                }
                GameplayController.spawnObjects_PL2[i] = Resources.Load<GameObject>("_Prefabs/" + playerDeck[i]);
            }

            playerID = 1;
            menuController.HideInventoryPanel();            
        }
        for (int i = 0; i < deckCards.Count; i++)
        {
            deckCards[i].GetComponent<Image>().sprite = emptySlotImage;
            playerDeck[i] = null; 
        }
        
        simpleCoinCount = specialCoinCount = powerupCount = 0;
    }  
}
