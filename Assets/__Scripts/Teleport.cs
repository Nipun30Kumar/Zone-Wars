using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour {

    #region PUBLIC VARIABLES

    public GameObject spawnPositionPFX;
    public GameObject finalPositionPFX;

    #endregion

    #region PRIVATE VARIABLES

    private float teleportationDelay = 1f;

    private bool finalPositionAboutToSet = false;
    private bool finalPostionSet = false;

    private Vector3 initialPosition;
    private Vector3 finalPosition;

    private GameplayController gameplayController;

    #endregion
    
    void Start () {
        gameplayController = FindObjectOfType<GameplayController>();
        initialPosition = gameplayController.currentObjectInMotion.transform.position;
        StartCoroutine(TeleportCoin());
	}
	
	void Update () {
        if (Input.GetMouseButtonDown(0) && !finalPostionSet)
        {            
            finalPositionAboutToSet = true;
        }

        if(Input.GetMouseButtonUp(0) && finalPositionAboutToSet)
        {
            finalPositionAboutToSet = false;
            finalPostionSet = true;
            finalPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            finalPosition.z = 0f;            
        }
	}

    IEnumerator TeleportCoin()
    {
        GameplayController.playerTurnStarted = true;
        yield return new WaitUntil(() => finalPostionSet == true);
        //Instantiate the PFX at spawn point
        Instantiate(spawnPositionPFX, initialPosition, Quaternion.identity, gameplayController.currentObjectInMotion.transform.parent);
        gameplayController.currentObjectInMotion.GetComponent<SpriteRenderer>().enabled = false;

        yield return new WaitForSeconds(teleportationDelay);

        Instantiate(finalPositionPFX, finalPosition, Quaternion.identity, gameplayController.currentObjectInMotion.transform.parent);
        gameplayController.currentObjectInMotion.transform.position = finalPosition;
        gameplayController.currentObjectInMotion.GetComponent<SpriteRenderer>().enabled = true;
        
        GameplayController.allCoinsStopped = true;
        GameplayController.playerTurnEnded = true;
        gameplayController.teleported = true;
        Debug.Log("Teleportation ended");
    }
}
