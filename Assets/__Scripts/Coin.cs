using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour {

    public float minimumPower;
    public float maximumPower;
    public bool player1Coin, player2Coin = false;
    public bool isMonkCoin = false;
    public bool isBomb = false;
    public string CoinType;
    public int scoreValue;
    public bool coinInNeutralZone = false;
    public bool stopState = true;
    private bool incrementCount = false;

    Transform ownZone;
    GameplayController gameplayController;

    void Start()
    {
        scoreValue = 0;
        gameplayController = FindObjectOfType<GameplayController>();

        if (GameplayController.player1Active)
        {
            //Zones for player 1 coin
            player1Coin = true;
            ownZone = gameplayController.player1OwnZone;

        }
        else if (GameplayController.player2Active)
        {
            //Zones for player 2 coin
            player2Coin = true;
            ownZone = gameplayController.player2OwnZone;
        }
    }

    void Update()
    {
        #region -- COIN POSITION CHECKS --
        if (player1Coin)
        {
            Debug.Log("Player 1 coin");
            //Check bounds for player 1
            if (this.transform.localPosition.y > 4f && !coinInNeutralZone)
            {
                //Debug.Log("Player 1 coin in player 2's zone at : " + this.transform.localPosition.y);

                scoreValue = 2; // Coin is in opponent's region for player 1
            }
            else if (this.transform.localPosition.y <= 4f && !coinInNeutralZone)
            {
                //Debug.Log("Player 1 coin in player 1's zone at : " + this.transform.localPosition.y);

                scoreValue = 1; // Coin is in player 1's region itself
            }
        }
        else if (player2Coin)
        {
            Debug.Log("Player 2 coin");
            //Check bounds for player 2
            if (this.transform.localPosition.y < -4f && !coinInNeutralZone)
            {
                //Debug.Log("Player 2 coin in player 1's zone at : " + this.transform.localPosition.y);

                scoreValue = 2; // Coin is in opponent's region for player 2
            }
            else if (this.transform.localPosition.y >= -4f && !coinInNeutralZone)
            {
                //Debug.Log("Player 2 coin in player 2's zone at : " + this.transform.localPosition.y);

                scoreValue = 1; // Coin is in player 2's region itself
            }
        }
        else
        {
            Debug.Log("None of the players are active !!!");
        }
        #endregion

        #region -- COIN MOVEMENT STATE CHECKS --

        if (this.GetComponent<Rigidbody2D>().velocity.magnitude > 0.1f)
        {
            Debug.Log("Coin has been launched");
            stopState = false;
            GameplayController.playerTurnStarted = true;
        }

        if ((this.GetComponent<Rigidbody2D>().velocity.magnitude <= 0.1f) && !GameplayController.modeAI && !stopState)
        {
            Debug.Log("Coin velocity becomes 0");
            stopState = true;            
            GameplayController.playerTurnEnded = true;
        }

        if ((this.GetComponent<Rigidbody2D>().velocity.magnitude <= 0.1f) && GameplayController.modeAI && GameplayController.player2Active && GameplayController.playerSwipeStarted)
        {
            Debug.Log("AI has finished his move" + this.GetComponent<Rigidbody2D>().velocity.magnitude);
            GameplayController.playerTurnEnded = true;
            GameplayController.playerTurnStarted = false;
        }

        #endregion
    }

}
