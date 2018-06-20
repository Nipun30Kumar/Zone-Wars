using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPositionChecker : MonoBehaviour {

    GameplayController gameplayController;

    Transform ownZone;
    //Transform enemyZone;

    public bool player1Coin = false;
    public bool player2Coin = false;
    public bool coinInNeutralZone = false;

    Rigidbody2D coinRigidBody;

    void Start() {
        //Assign the zones to the coin based on the player turn
        gameplayController = FindObjectOfType<GameplayController>();
        coinRigidBody = this.GetComponent<Rigidbody2D>();

        if (GameplayController.player1Active)
        {
            //Zones for player 1 coin
            player1Coin = true;
            ownZone = gameplayController.player1OwnZone;

        }
        else if (GameplayController.player2Active) {
            //Zones for player 2 coin
            player2Coin = true;
            ownZone = gameplayController.player2OwnZone;
        }
    }

    public void CheckScoreValue() {

        if (player1Coin)
        {
            Debug.Log("Player 1 coin");
            //Check bounds for player 1
            if (this.transform.localPosition.y > 4f && !coinInNeutralZone)
            {
                //Debug.Log("Player 1 coin in player 2's zone at : " + this.transform.localPosition.y);

                gameplayController.scorePL1 += 1; // Coin is in opponent's region for player 1
            }
            else if(this.transform.localPosition.y <= 4f && !coinInNeutralZone)
            {
                //Debug.Log("Player 1 coin in player 1's zone at : " + this.transform.localPosition.y);

                gameplayController.scorePL1 -= 1; // Coin is in player 1's region itself
            }
        }
        else if(player2Coin)
        {
            Debug.Log("Player 2 coin");
            //Check bounds for player 2
            if (this.transform.localPosition.y < -4f && !coinInNeutralZone)
            {
                //Debug.Log("Player 2 coin in player 1's zone at : " + this.transform.localPosition.y);

                gameplayController.scorePL2 += 1; // Coin is in opponent's region for player 2
            }
            else if(this.transform.localPosition.y >= -4f && !coinInNeutralZone)
            {
                //Debug.Log("Player 2 coin in player 2's zone at : " + this.transform.localPosition.y);

                gameplayController.scorePL2 -= 1; // Coin is in player 2's region itself
            }
        }
        else {
            Debug.Log("None of the players are active !!!");
        }
    }
	
	
}
