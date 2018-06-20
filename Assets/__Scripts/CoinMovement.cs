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
            #region Editor Input handling
#if UNITY_EDITOR
            if (Input.GetMouseButtonDown(0))
            {
                GameplayController.playerTurnEnded = false;
                GameplayController.playerTurnStarted = true;

                //Store initial values
                startPos = Input.mousePosition;
                startTime = Time.time;
            }           

            if (Input.GetMouseButtonUp(0) && !GameplayController.playerSwipeStarted)
            {
                GameplayController.playerSwipeStarted = true;
                //Get end values
                Vector3 endPos = Input.mousePosition;
                float endTime = Time.time;

                //Makes the input pixel density independent
                startPos = Camera.main.ScreenToWorldPoint(startPos);
                endPos = Camera.main.ScreenToWorldPoint(endPos);

                //The duration of the swipe
                float duration = endTime - startTime;

                //The direction of the swipe
                Vector3 dir = endPos - startPos;

                //The distance of the swipe
                float distance = dir.magnitude;

                //Faster or longer swipes give higher power
                float power = (distance / Mathf.Clamp(duration, minDuration, maxDuration)) * powerFactor;

                Debug.Log("Power : " + power);
                Debug.Log("Duration of swipe : " + Mathf.Clamp(duration, minDuration, maxDuration));
                //take the direction from the swipe. length of the vector is the power
                Vector3 velocity = (transform.rotation * dir).normalized * power;
                //Debug.Log("Velocity : " + velocity);

                //this.GetComponent<Rigidbody2D>().AddForce(dir * power *10);
                this.GetComponent<Rigidbody2D>().velocity = velocity;

                //coinVelocity = this.GetComponent<Rigidbody2D>().velocity;
                checkVelocity = true;
                //Debug.Log("Checking velocity!");

            }
            //Debug.Log("check velocity : " + checkVelocity );
            //Debug.Log(this.GetComponent<Rigidbody2D>().velocity.magnitude);
            if ((this.GetComponent<Rigidbody2D>().velocity.magnitude <= 0.1f) && checkVelocity && !GameplayController.modeAI)
            {
                Debug.Log("Coin velocity becomes 0");
                checkVelocity = false;                
                GameplayController.playerTurnEnded = true;
                GameplayController.playerTurnStarted = false;                   
            }

            if ((this.GetComponent<Rigidbody2D>().velocity.magnitude <= 0.1f) && GameplayController.modeAI && GameplayController.player2Active && GameplayController.playerSwipeStarted)
            {
                Debug.Log("AI has finished his move" + this.GetComponent<Rigidbody2D>().velocity.magnitude);
                GameplayController.playerTurnEnded = true;
                GameplayController.playerTurnStarted = false;
            }
#endif
            #endregion
            #region Touch Input Handling
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                GameplayController.playerTurnEnded = false;
                GameplayController.playerTurnStarted = true;

                //Store initial values
                startPos = Input.mousePosition;
                startTime = Time.time;
            }
                        
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended && !GameplayController.playerSwipeStarted)
            {
                GameplayController.playerSwipeStarted = true;
                //Get end values
                Vector3 endPos = Input.mousePosition;
                float endTime = Time.time;

                //Makes the input pixel density independent
                startPos = Camera.main.ScreenToWorldPoint(startPos);
                endPos = Camera.main.ScreenToWorldPoint(endPos);

                //The duration of the swipe
                float duration = endTime - startTime;

                //The direction of the swipe
                Vector3 dir = endPos - startPos;

                //The distance of the swipe
                float distance = dir.magnitude;

                //Faster or longer swipes give higher power
                float power = (distance / Mathf.Clamp(duration, minDuration, maxDuration)) * powerFactor;

                //Debug.Log("Power : " + power);
                
                //take the direction from the swipe. length of the vector is the power
                Vector3 velocity = (transform.rotation * dir).normalized * power;
                //Debug.Log("Velocity : " + velocity);

                this.GetComponent<Rigidbody2D>().velocity = velocity;

                //coinVelocity = this.GetComponent<Rigidbody2D>().velocity;
                checkVelocity = true;
                //Debug.Log("Checking velocity!");

            }
            //Debug.Log("check velocity : " + checkVelocity );
            //Debug.Log(this.GetComponent<Rigidbody2D>().velocity.magnitude);
            if ((this.GetComponent<Rigidbody2D>().velocity.magnitude <= 0.1f) && checkVelocity)
            {
                //Debug.Log("Velocity became zero !");
                checkVelocity = false;
                GameplayController.playerTurnEnded = true;
                GameplayController.playerTurnStarted = false;
            }
            #endregion
        }
    }
}
