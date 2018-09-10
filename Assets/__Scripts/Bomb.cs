using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bomb : MonoBehaviour {

    public GameObject explosionEffect;
    public int playerID;
    public CircleCollider2D explosionRegion;

    public List<Coin> coinList;    // The list of coins which are in the region of the explosion.

    Coin coinTrait;
    GameplayController gameplayController;

    
    void OnEnable()
    {
        coinList = new List<Coin>();
        coinTrait = this.GetComponentInParent<Coin>();
        coinTrait.isBomb = true;
        gameplayController = FindObjectOfType<GameplayController>();
        playerID = 0;
    }

    public void Initiate()
    {
        StartCoroutine(PerformExplosion());
    }

    public void ReturnControl()
    {
        Debug.Log("Returning to gameplay controller");
       
    }

    public void OnTriggerEnter2D(Collider2D coin)
    {
        if (coin.tag == "Coin")
        {
            coinList.Add(coin.GetComponent<Coin>());
        }
    }

    private void OnTriggerExit2D(Collider2D coin)
    {
        if (coin.tag == "Coin")
        {
            coinList.Remove(coin.GetComponent<Coin>());
        }
    }

    public IEnumerator PerformExplosion()
    {
        Debug.Log("Waiting till player turn ends");
        yield return new WaitUntil(() => (GameplayController.allCoinsStopped == true && GameplayController.playerTurnEnded == true));
        Debug.Log("Turn ended");

        // Player's turn has ended. Perform conversion. Initiate a wait in the gameplay controller.        
        Instantiate(explosionEffect, gameplayController.currentObjectInMotion.transform.position, Quaternion.identity, gameplayController.currentObjectInMotion.transform.parent);
        this.GetComponentInParent<SpriteRenderer>().sprite = null;
        gameplayController.exploding = true;

        //List<Coin> destructionList = new List<Coin>();
        Debug.Log("Exploding");
        foreach (Coin coin in coinList.ToArray())
        {
            if (coinTrait.player1Coin && coin.tag == "Coin" && coin.player2Coin)
            {
                Debug.Log("Bomb from player 1, Destroying all polayer 2 coins in the explosion region.");
                //destructionList.Add(coin);
                coin.gameObject.SetActive(false);
                gameplayController.placedCoins.Remove(coin.gameObject);                
                //Destroy(coin.gameObject);
            }
            else if (coinTrait.player2Coin && coin.tag == "Coin" && coin.player1Coin)
            {
                Debug.Log("Bomb from player 2, Destroying all polayer 1 coins in the explosion region.");
                //destructionList.Add(coin);
                coin.gameObject.SetActive(false);
                gameplayController.placedCoins.Remove(coin.gameObject);
                //Destroy(coin.gameObject);
            }            
        }

        // All coins converted. Destroy the monk coin accompanied with a PFX. Return control to gameplay controller.
        GameplayController.allCoinsStopped = true;
        GameplayController.playerTurnEnded = true;
        gameplayController.exploding = false;        
    }    
}
