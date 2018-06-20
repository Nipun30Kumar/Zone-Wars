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

    public GameObject[] spawnObjects_PL1;
    public GameObject[] spawnObjects_PL2;
    public GameObject currentObjectInMotion;
    public GameObject gameHelpPanel;
    public GameObject pointsPanel;
    public GameObject blackBG;
    public GameObject gameOverPanel;
    public GameObject glueRegion;
    public GameObject greaseRegion;
    public List<GameObject> placedCoins;

    public TextMeshProUGUI pl1Score, pl2Score, pl1Moves, pl2Moves;
    public TextMeshProUGUI gameOverPanelScorePL1, gameOverPanelScorePL2;

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
    public bool helpRead = false;

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
    }

	void Update () {        
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
                player1Active = true;
                player2Active = false;
                // Instantiate random coin for player 1
                playerTurnEnded = false;
                Turn = true;  // Set the next turn for player 2.
                currentObjectInMotion =  Instantiate(spawnObjects_PL1[Random.Range(0, spawnObjects_PL1.Length)],
                                                    player1SpawnPos.position, 
                                                    Quaternion.identity,
                                                    player1SpawnPos) as GameObject;
                
                yield return new WaitUntil(() => playerTurnStarted == true);
                turnCountPL1--;
            }
            else if (Turn && playerTurnEnded == true)
            {
                player1Active = false;
                player2Active = true;
                // Instantiate random coin for player 2
                playerTurnEnded = false;
                Turn = false;  // Set the next turn for player 1.
                currentObjectInMotion = Instantiate(spawnObjects_PL2[Random.Range(0, spawnObjects_PL2.Length)],
                                                    player2SpawnPos.position, 
                                                    Quaternion.identity, 
                                                    player2SpawnPos) as GameObject;
                
                yield return new WaitUntil(() => playerTurnStarted == true);
                turnCountPL2--;
            }
            yield return new WaitUntil(() => playerSwipeStarted == true);               
            //Debug.Log("Waiting for player turn to end !!!");
            yield return new WaitUntil(()=> playerTurnEnded == true);
            scorePL1 = scorePL2 = 0;
            currentObjectInMotion.GetComponent<CoinPositionChecker>().CheckScoreValue(); // Check the score value for the current coin that was in motion.
            foreach(GameObject coins in placedCoins)
            {
                coins.GetComponent<CoinPositionChecker>().CheckScoreValue();
            }
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
            currentObjectInMotion.GetComponent<CoinPositionChecker>().CheckScoreValue(); // Check the score value for the current coin that was in motion.
            foreach (GameObject coins in placedCoins)
            {
                coins.GetComponent<CoinPositionChecker>().CheckScoreValue();
            }
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
}
