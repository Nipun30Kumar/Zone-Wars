using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Glue : MonoBehaviour {

    #region PUBLIC VARIABLES

    public Animation coinExplosionAnimation;
    public SpriteRenderer powerUpSprite;

    #endregion

    #region PRIVATE VARIABLES

    private float explosionDelay = 2.5f;
    private bool glue = false;
    private bool grease = false;
    private GameplayController gameplayController;

    #endregion

    void Start()
    {       
        gameplayController = FindObjectOfType<GameplayController>();
        gameplayController.powerUpUsed = false;
        if (gameplayController.gluePowerup)
        {
            glue = true;
        }
        else if (gameplayController.greasePowerup)
        {
            grease = true;
            powerUpSprite.color = Color.green;
        }
        StartCoroutine(ActivatePowerup());
    }

    IEnumerator ActivatePowerup()
    {
        yield return new WaitUntil(() => GameplayController.playerTurnStarted == true);
        yield return new WaitUntil(() => GameplayController.playerTurnEnded == true);       //WAIT TILL THE COIN HAS STOPPED

        coinExplosionAnimation.Play();      // The coin scales to zero as the glue or grease region scales to 1.
        yield return new WaitForSeconds(explosionDelay);
        
        gameplayController.currentObjectInMotion.GetComponent<SpriteRenderer>().enabled = false;
        gameplayController.powerUpUsed = true;
        GameplayController.allCoinsStopped = true;
        GameplayController.playerTurnEnded = true;
        Debug.Log("Powerup explosion complete");
    }
}
