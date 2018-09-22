using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeutralZone : MonoBehaviour {

    GameplayController gameplayController;
    Coin coin;

	// Use this for initialization
	void Awake () {
        gameplayController = FindObjectOfType <GameplayController>();
    }
	
	public void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.tag == "Coin")
        {
            Coin coinTrait = collider.gameObject.GetComponent<Coin>();

            if (coinTrait.player1Coin)
            {
                coinTrait.coinInNeutralZone = true;
                Debug.Log("Increasing player 1's score");
                gameplayController.neutralZoneScorePL1 += 3;
                Debug.Log("Player 1 score : " + gameplayController.scorePL1.ToString());
            }
            else if (coinTrait.player2Coin)
            {
                coinTrait.coinInNeutralZone = true;
                Debug.Log("Increasing player 2's score");
                gameplayController.neutralZoneScorePL2 += 3;
            }
            else
            {
                Debug.Log("Unknown object has entered the neutral zone !!");
            }
        }
        //gameplayController.UpdateHUD();
    }

    public void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.tag == "Coin")
        {
            Coin coinTrait = collider.gameObject.GetComponent<Coin>();

            if (coinTrait.player1Coin)
            {
                coinTrait.coinInNeutralZone = false;
                gameplayController.neutralZoneScorePL1 -= 3;
            }
            else if (coinTrait.player2Coin)
            {
                coinTrait.coinInNeutralZone = false;
                gameplayController.neutralZoneScorePL2 -= 3;
            }
            else
            {
                Debug.Log("Unknown object has left the neutral zone !!");
            }
        }
        //gameplayController.UpdateHUD();
    }
}
