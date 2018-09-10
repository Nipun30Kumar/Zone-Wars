using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CoinMovement : MonoBehaviour {

    Vector2 coinVelocity;
    Vector3 startPos;

    float startTime;
    float maxDuration = 5f;
    float minDuration = 1f;
    [SerializeField]
    float powerFactor;

    bool checkVelocity = false;
    

    void Update()
    {
        if (!EventSystem.current.IsPointerOverGameObject()) {

            if(this.GetComponent<Rigidbody2D>().velocity.magnitude > 0.1f)
            {
                Debug.Log("Coin has been launched");
                checkVelocity = true;
                GameplayController.playerTurnStarted = true;
            }

            if ((this.GetComponent<Rigidbody2D>().velocity.magnitude <= 0.1f) && !GameplayController.modeAI && checkVelocity)
            {
                Debug.Log("Coin velocity becomes 0");
                checkVelocity = false;
                GameplayController.playerTurnEnded = true;
                //GameplayController.playerTurnStarted = false;                   
            }

            if ((this.GetComponent<Rigidbody2D>().velocity.magnitude <= 0.1f) && GameplayController.modeAI && GameplayController.player2Active && GameplayController.playerSwipeStarted)
            {
                Debug.Log("AI has finished his move" + this.GetComponent<Rigidbody2D>().velocity.magnitude);
                GameplayController.playerTurnEnded = true;
                GameplayController.playerTurnStarted = false;
            }
   
        }
    }
}
