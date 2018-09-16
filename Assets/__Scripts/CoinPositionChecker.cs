using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPositionChecker : MonoBehaviour {

    GameplayController gameplayController;
    ScoreManager scoreManager;
    
    public bool coinStopped = false;
    Rigidbody2D coinRigidBody;
    Coin coin;

    void Start() {
        //Assign the zones to the coin based on the player turn
        gameplayController = FindObjectOfType<GameplayController>();
        scoreManager = FindObjectOfType<ScoreManager>();
        coinRigidBody = this.GetComponent<Rigidbody2D>();
        coin = this.GetComponent<Coin>();
    }

}
