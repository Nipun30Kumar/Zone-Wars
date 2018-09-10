using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour {

    GameplayController gameplayController;

    public static int counter;

    void Start()
    {
        gameplayController = FindObjectOfType<GameplayController>();
    }
	
    void Update()
    {
        if (!GameplayController.allCoinsStopped && counter == gameplayController.placedCoins.Count)
        {
            GameplayController.allCoinsStopped = true;
            counter = 0;
        }
    }
}
