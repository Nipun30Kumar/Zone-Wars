using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeutralZone : MonoBehaviour {

    GameplayController gameplayController;
    Coin coin;

	// Use this for initialization
	void Start () {
        gameplayController = FindObjectOfType <GameplayController>();
        coin = this.GetComponent<Coin>();
    }
	
	public void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.tag == "Coin")
        {
            Coin coinTrait = collider.gameObject.GetComponent<Coin>();
            CoinPositionChecker coinPosition = collider.gameObject.GetComponent<CoinPositionChecker>();

            if (coinTrait.player1Coin)
            {
                coin.coinInNeutralZone = true;
                Debug.Log("Increasing player 1's score");
                gameplayController.neutralZoneScorePL1 += 3;
                Debug.Log("Player 1 score : " + gameplayController.scorePL1.ToString());
            }
            else if (coinTrait.player2Coin)
            {
                coin.coinInNeutralZone = true;
                Debug.Log("Increasing player 2's score");
                gameplayController.neutralZoneScorePL2 += 3;
            }
            else
            {
                Debug.Log("Unknown object has entered the neutral zone !!");
            }
        }
        gameplayController.UpdateHUD();
    }

    public void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.tag == "Coin")
        {
            Coin coinTrait = collider.gameObject.GetComponent<Coin>();
            CoinPositionChecker coinPosition = collider.gameObject.GetComponent<CoinPositionChecker>();
            if (coinTrait.player1Coin)
            {
                coin.coinInNeutralZone = false;
                gameplayController.neutralZoneScorePL1 -= 3;
            }
            else if (coinTrait.player2Coin)
            {
                coin.coinInNeutralZone = false;
                gameplayController.neutralZoneScorePL2 -= 3;
            }
            else
            {
                Debug.Log("Unknown object has left the neutral zone !!");
            }
        }
        gameplayController.UpdateHUD();
    }
}
