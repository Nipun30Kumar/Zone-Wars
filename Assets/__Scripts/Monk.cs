using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monk : MonoBehaviour {

    public GameObject conversionEffect;
    public int playerID; 
    public CircleCollider2D conversionRegion;
    Coin coinTrait;
    GameplayController gameplayController;

    private List<Coin> coinList;

    void OnEnable()
    {
        coinList = new List<Coin>();
        coinTrait = this.GetComponentInParent<Coin>();
        coinTrait.isMonkCoin = true;
        gameplayController = FindObjectOfType<GameplayController>();
        playerID = 0;
    }

    public void Initiate()
    {
        StartCoroutine(PerformConversion());
    }

    public void ReturnControl()
    {
        
    }

	public void OnTriggerEnter2D(Collider2D coin)
    {
        if(coin.tag == "Coin")
        {
            coinList.Add(coin.GetComponent<Coin>());
        }
    }

    private void OnTriggerExit2D(Collider2D coin)
    {
        if(coin.tag == "Coin")
        {
            coinList.Remove(coin.GetComponent<Coin>());
        }
    }

    public IEnumerator PerformConversion()
    {
        Debug.Log("Waiting till player turn ends");
        yield return new WaitUntil(() => (GameplayController.allCoinsStopped == true && GameplayController.playerTurnEnded == true));
        Debug.Log("Turn ended");

        // Player's turn has ended. Perform conversion. Initiate a wait in the gameplay controller.

        gameplayController.converting = true;

        Debug.Log("Converting");
        foreach (Coin coin in coinList)
        {
            if (coinTrait.player1Coin && coin.tag == "Coin" && coin.player2Coin)
            {
                coin.player1Coin = true;
                coin.player2Coin = false;
                Instantiate(conversionEffect, coin.transform.position, Quaternion.identity, coin.transform.parent);
                if(coin.CoinType != "Teleport")
                    coin.GetComponent<SpriteRenderer>().color = Color.yellow;
            }
            else if (coinTrait.player2Coin && coin.tag == "Coin" && coin.player1Coin)
            {
                coin.player1Coin = false;
                coin.player2Coin = true;
                Instantiate(conversionEffect, coin.transform.position, Quaternion.identity, coin.transform.parent);
                if (coin.CoinType != "Teleport")
                    coin.GetComponent<SpriteRenderer>().color = Color.red;
            }
        }

        yield return new WaitForSeconds(2f);
        // All coins converted. Destroy the monk coin accompanied with a PFX. Return control to gameplay controller.
        Debug.Log("All coins converted in the region.");
        gameplayController.converting = false;
    }
}
