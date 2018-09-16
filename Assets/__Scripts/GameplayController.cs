using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameplayController : MonoBehaviour {

    public Transform player1SpawnPos;
    public Transform player2SpawnPos;
    public Transform helpPanelPos;
    public Transform panelFinalTweenPosition;
    public Transform player1OwnZone;
    public Transform player2OwnZone;

    public static int size;
    public static GameObject[] spawnObjects_PL1 = new GameObject[5];
    public static GameObject[] spawnObjects_PL2 = new GameObject[5];
    public GameObject currentObjectInMotion;
    public GameObject gameHelpPanel;
    public GameObject pointsPanel;
    public GameObject blackBG;
    public GameObject gameOverPanel;
    public GameObject glueRegion;
    public GameObject greaseRegion;
    public GameObject player1AimAssist;
    public GameObject player2AimAssist;
    public List<GameObject> placedCoins;

    public TextMeshProUGUI pl1Score, pl2Score, pl1Moves, pl2Moves;
    public TextMeshProUGUI gameOverPanelScorePL1, gameOverPanelScorePL2;

    public BoxCollider2D colliderPlayer1;
    public BoxCollider2D colliderPlayer2;

    public static int turnCount;
    public int turnCountPL1;
    public int turnCountPL2;
    public int scorePL1;
    public int scorePL2;
    public int neutralZoneScorePL1;
    public int neutralZoneScorePL2;
    int initialMoveCount;

    public bool switchTurn = false;
    public static bool playerTurnEnded = true;
    public static bool playerTurnStarted = false;
    public static bool playerSwipeStarted = false;
    public static bool modeAI = false;
    public static bool player1Active = false;
    public static bool player2Active = false;
    public static bool allCoinsStopped = true;
    public static bool currentCoinLaunched = false;
    public bool helpRead = false;
    public bool requireAimAssist = false;

    // MONK COIN
    private bool monkCoin = false;
    public bool converting = false;

    // BOMB COIN
    private bool bombCoin = false;
    public bool exploding = false;

    // GHOST COIN
    private bool ghostCoin = false;

    // TELEPORT  COIN
    private bool teleportCoin = false;
    public bool teleported = false;

    // GLUE POWERUP
    public bool gluePowerup = false;
    public bool powerUpUsed = false;

    // GREASE POWERUP
    public bool greasePowerup = false;

    // AI related variables
    public Vector2 destinationVectorAI;

    void Start () {
        LeanTween.move(gameHelpPanel, helpPanelPos, 0.25f);
        blackBG.SetActive(true);
        turnCountPL1 = turnCountPL2 = initialMoveCount = turnCount / 2;
        pl1Moves.text = "MOVES LEFT : " + turnCountPL1.ToString();
        pl2Moves.text = "MOVES LEFT : " + turnCountPL2.ToString();
        pl1Score.text = "SCORE : 0";
        pl2Score.text = "SCORE : 0";
        scorePL1 = scorePL2 = 0;
        placedCoins = new List<GameObject>();
        for (int i = 0; i < spawnObjects_PL1.Length; i++)
        {
            Debug.Log("List 1 , Element " + i + "    Coin : " + spawnObjects_PL1[i].name);
            Debug.Log("List 2 , Element " + i + "    Coin : " + spawnObjects_PL2[i].name);
        }    
    }

    public void StartGameCoroutine() {
        if(helpRead && !modeAI)
        {
            helpRead = false;
            StartCoroutine(Start2PlayerMode(switchTurn));
        }
        else if(helpRead && modeAI)
        {
            helpRead = false;
            StartCoroutine(Start1PlayerMode(switchTurn));
        }
    }

    IEnumerator Start2PlayerMode(bool Turn)
    {
        while (turnCount > 0)
        {
            if (!Turn && playerTurnEnded == true)
            {
                //Debug.Log("Player 1 TURN!");
                player1Active = true;
                player2Active = false;                
                playerTurnEnded = false;
                Turn = true;  // Set the next turn for player 2.

                colliderPlayer1.gameObject.GetComponent<BoxCollider2D>().enabled = false;     // Deactivate the launch area collider for player 1.
                colliderPlayer2.gameObject.GetComponent<BoxCollider2D>().enabled = false;      // Activate the launch area collider for player 2.

                // Instantiate random coin for player 1
                currentObjectInMotion =  Instantiate(spawnObjects_PL1[Random.Range(0, spawnObjects_PL1.Length)],
                                                    player1SpawnPos.position, 
                                                    Quaternion.identity,
                                                    player1SpawnPos) as GameObject;

                monkCoin = bombCoin = gluePowerup = greasePowerup = teleportCoin = false;
                CheckCoinType();

                if (requireAimAssist && currentObjectInMotion != null)
                {
                    player1AimAssist.SetActive(true);
                    player1AimAssist.GetComponent<AimAssist>().objectInMotion = currentObjectInMotion;
                }

                yield return new WaitUntil(() => playerTurnStarted == true);
                turnCountPL1--;
                yield return new WaitUntil(() => playerTurnEnded == true);
                if (requireAimAssist)
                {
                    player1AimAssist.GetComponent<AimAssist>().gameObject.SetActive(false);
                    player1AimAssist.GetComponent<AimAssist>().objectInMotion = null;
                }
            }
            else if (Turn && playerTurnEnded == true)
            {
                //Debug.Log("Player 2 TURN!");
                player1Active = false;
                player2Active = true;                
                playerTurnEnded = false;
                Turn = false;  // Set the next turn for player 1.

                colliderPlayer2.gameObject.GetComponent<BoxCollider2D>().enabled = false;    // Deactivate the launch area collider for player 2.
                colliderPlayer1.gameObject.GetComponent<BoxCollider2D>().enabled = false;   // Activate the launch area collider for player 1.

                // Instantiate random coin for player 2
                currentObjectInMotion = Instantiate(spawnObjects_PL2[Random.Range(0, spawnObjects_PL2.Length)],
                                                    player2SpawnPos.position, 
                                                    Quaternion.identity, 
                                                    player2SpawnPos) as GameObject;

                monkCoin = bombCoin = gluePowerup = greasePowerup = teleportCoin = false;
                CheckCoinType();

                if (requireAimAssist && currentObjectInMotion != null)
                {
                    player2AimAssist.SetActive(true);
                    player2AimAssist.GetComponent<AimAssist>().objectInMotion = currentObjectInMotion;
                }
                
                yield return new WaitUntil(() => playerTurnStarted == true);
                turnCountPL2--;
                yield return new WaitUntil(() => playerTurnEnded == true);
                if (requireAimAssist)
                {
                    player2AimAssist.GetComponent<AimAssist>().gameObject.SetActive(false);
                    player2AimAssist.GetComponent<AimAssist>().objectInMotion = null;
                }
            }


            if (currentObjectInMotion != null && !monkCoin && !bombCoin && !gluePowerup && !greasePowerup && !teleportCoin)
            {
                placedCoins.Add(currentObjectInMotion);
            }

            if (requireAimAssist)
            {
                yield return new WaitUntil(() => currentCoinLaunched == true);  // THIS IS SET TO TRUE ONLY WHEN A COIN IN LAUNCHED. VELOCITY => RIGIDBODY.
                currentCoinLaunched = false;
            }

            #region -- COIN MOVEMENT STOP CHECK --
            //Debug.Log("Checking when all coins have stopped");            
            foreach (GameObject coin in placedCoins)
            {
                if(coin.GetComponent<Coin>().stopState == false)
                {
                    yield return new WaitUntil(() => coin.GetComponent<Coin>().stopState == true);
                }
            }
            //Debug.Log("All coins have stopped");
            #endregion

            #region -- SPECIAL COINS HANDLING --

            // MONK COIN HANDLING
            if (monkCoin)
            {
                monkCoin = false;
                yield return new WaitUntil(() => converting == false);
                Destroy(currentObjectInMotion);
            }

            // BOMB COIN HANDLING
            if (bombCoin)
            {
                bombCoin = false;
                Debug.Log("Waiting until explosion is complete");                
                yield return new WaitUntil(() => exploding == false);
                yield return new WaitForSeconds(1.5f);
                Destroy(currentObjectInMotion);
                //Debug.Log("Bomb coin destroyed");
            }

            // GHOST COIN HANDLING
            if (ghostCoin)
            {
                ghostCoin = false;
            }

            // TELEPORT COIN HANDLING
            if (teleportCoin)
            {
                teleportCoin = false;
                yield return new WaitUntil(() => teleported == true);
                teleported = false;
                Debug.Log("Teleportation handled !!");
            }

            // GLUE COIN HANDLING
            if (gluePowerup)
            {
                gluePowerup = false;
                yield return new WaitUntil(() => powerUpUsed == true);
                powerUpUsed = false;
                Debug.Log("Glue Powerup Used");
            }

            // GREASE COIN HANDLING
            if (greasePowerup)
            {
                greasePowerup = false;
                yield return new WaitUntil(() => powerUpUsed == true);
                powerUpUsed = false;
                Debug.Log("Glue Powerup Used");
            }

            #endregion

            if (currentObjectInMotion != null)
            {
                currentObjectInMotion.GetComponent<CoinMovement>().enabled = false;
            }

            #region -- SCORE CALCULATIONS --
            // RESETTING THE SCORE TO CALCULATE BASED ON NEW POSITIONS OF THE COINS.
            scorePL1 = scorePL2 = 0;

            foreach (GameObject coin in placedCoins)
            {
                if (coin.GetComponent<Coin>().player1Coin)
                {
                    scorePL1 += coin.GetComponent<Coin>().scoreValue;
                }
                else if (coin.GetComponent<Coin>().player2Coin)
                {
                    scorePL2 += coin.GetComponent<Coin>().scoreValue;
                }
            }
            #endregion

            UpdateHUD();                      
            turnCount--;
            
            //Debug.Log("CYCLE COMPLETE");
            if (turnCount == 0 && playerTurnEnded == true)
            {
                gameOverPanel.layer = 31;
                gameOverPanelScorePL1.text = scorePL1.ToString("00");
                gameOverPanelScorePL2.text = scorePL2.ToString("00");
                gameOverPanel.SetActive(true);
                StopAllCoroutines();
            }
        }
    }

    IEnumerator Start1PlayerMode(bool Turn)
    {
        while (turnCount > 0)
        {
            if (!Turn && playerTurnEnded == true)
            {
                player1Active = true;
                player2Active = false;
                // Instantiate random coin for player 1
                playerTurnEnded = false;
                Turn = true;  // Set the next turn for the AI.
                currentObjectInMotion = Instantiate(spawnObjects_PL1[Random.Range(0, spawnObjects_PL1.Length)],
                                                    player1SpawnPos.position,
                                                    Quaternion.identity,
                                                    player1SpawnPos) as GameObject;

                yield return new WaitUntil(() => playerTurnStarted == true);
                turnCountPL1--;
            }
            else if (Turn && playerTurnEnded == true)
            {
                // PASSIVE AI CONTROL

                player1Active = false;
                player2Active = true;
                // Instantiate random coin for the AI.
                playerTurnEnded = false;
                Turn = false;  // Set the next turn for player 1.
                currentObjectInMotion = Instantiate(spawnObjects_PL2[Random.Range(0, spawnObjects_PL2.Length)],
                                                    player2SpawnPos.position,
                                                    Quaternion.identity,
                                                    player2SpawnPos) as GameObject;

                playerTurnStarted = true;
                yield return new WaitForSeconds(2.5f);
                ShootCoinPassiveAI(currentObjectInMotion);
                playerSwipeStarted = true;
                turnCountPL2--;
            }
            if (player1Active)
            {
                yield return new WaitUntil(() => playerSwipeStarted == true);
            }
            
            // Wait for the end of a player's turn.

            yield return new WaitUntil(() => playerTurnEnded == true);

            //Score Calculations
            scorePL1 = scorePL2 = 0;
           
            scorePL1 += neutralZoneScorePL1;
            scorePL2 += neutralZoneScorePL2;
            currentObjectInMotion.GetComponent<CoinMovement>().enabled = false;
            if (currentObjectInMotion != null)
            {
                placedCoins.Add(currentObjectInMotion);
            }
            playerSwipeStarted = false;
            UpdateHUD();
            turnCount--;
            if (turnCount == 0 && playerTurnEnded == true)
            {
                gameOverPanel.layer = 31;
                gameOverPanelScorePL1.text = scorePL1.ToString("00");
                gameOverPanelScorePL2.text = scorePL2.ToString("00");
                gameOverPanel.SetActive(true);
                StopAllCoroutines();
            }
        }
    }

    public void ReloadLevel() {
        ResetLevelParameters();
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("Gameplay");
    }

    public void UpdateHUD() {
        pl1Moves.text = "MOVES LEFT : " + turnCountPL1.ToString();
        pl2Moves.text = "MOVES LEFT : " + turnCountPL2.ToString();

        pl1Score.text = "SCORE : " + scorePL1.ToString();
        pl2Score.text = "SCORE : " + scorePL2.ToString();
    }

    void ResetLevelParameters()
    {
        turnCountPL1 = turnCountPL2 = initialMoveCount;
        turnCount = initialMoveCount * 2;
        scorePL1 = scorePL2 = 0;
        placedCoins.Clear();
    }

    void ShootCoinPassiveAI(GameObject coin)
    {
        destinationVectorAI = new Vector2(Random.Range(-2.5f, 2.5f), Random.Range(-3.5f, -2f));
        coin.GetComponent<Rigidbody2D>().AddForce(destinationVectorAI * 10f, ForceMode2D.Impulse) ;
    }

    public int GetCurrentUser()
    {
        int id = 0;
        if (player1Active)
        {
            id = 1;
        }
        else if(player2Active)
        {
            id = 2;
        }
        return id;
    }

    public void CheckCoinType()
    {
        switch (currentObjectInMotion.GetComponent<Coin>().CoinType)
        {
            case "Monk":
                Debug.Log("The current coin is a monk coin");
                currentObjectInMotion.GetComponentInChildren<Monk>().Initiate();
                monkCoin = true;
                requireAimAssist = true;
                break;

            case "Bomb":
                Debug.Log("The current coin is a bomb coin");
                currentObjectInMotion.GetComponentInChildren<Bomb>().Initiate();
                bombCoin = true;
                requireAimAssist = true;
                break;

            case "Ghost":
                Debug.Log("The current coin is a ghost coin");
                ghostCoin = true;
                requireAimAssist = true;
                break;

            case "Teleport":
                Debug.Log("The current coin is a teleport coin");
                teleportCoin = true;
                requireAimAssist = false;
                break;

            case "Glue":
                Debug.Log("The current coin is a GLUE powerup");
                gluePowerup = true;
                requireAimAssist = true;
                break;

            case "Grease":
                Debug.Log("The current coin is a GREASE powerup");
                greasePowerup = true;
                requireAimAssist = true;
                break;

            default:
                requireAimAssist = true;
                Debug.Log("The current coin is a simple coin");
                break;
        }
        
    }
}