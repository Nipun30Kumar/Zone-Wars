using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayControllerV2 : MonoBehaviour {

    public const int MAX_PLAYER = 2;
    public const float MAXIMUM_AIM_ANGLE = 165;
    public const float MINIMUM_AIM_ANGLE = 15;

    public const float MINIMUM_SHOOT_POWER = 10f;
    public const float MAXIMUM_SHOOT_POWER = 10f;

    public static int TurnCount;
    public static GameplayControllerV2 Instance;
    [SerializeField] Player[] players;

    [SerializeField] Gamestate gamestate;
    [SerializeField] Transform[] playerSpawnPos;
    [SerializeField] BoxCollider2D[] playerOwnZones;
    [SerializeField] int[] scores;

    [SerializeField] CircleCollider2D neutralZone;
    [SerializeField] BaseCoin baseCoinPrefab;

    public static GameObject[] spawnObjects_PL1 = new GameObject[5];
    public static GameObject[] spawnObjects_PL2 = new GameObject[5];

    public BaseCoin currentCoin;
    public List<BaseCoin> placedCoins;

    private int currentTurn = -1;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    private void Start() {
        TurnCount = 5;
        currentTurn = -1;
        scores = new int[2];

        placedCoins = new List<BaseCoin>();
        InitializePlayers();
    }

    private void ChangeTurn() {
        currentTurn += 1;
        currentTurn %= MAX_PLAYER;
        currentCoin = GetBaseCoin(playerSpawnPos[currentTurn].position);
        currentCoin.gameObject.layer = players[currentTurn].gameObject.layer;
        currentCoin.Initialize(currentTurn.ToString());
        players[currentTurn].MyTurn();
    }

    private void Update() {

    }

    private void InitializePlayers() {
        players[0].transform.position = playerSpawnPos[0].position;
        players[1].transform.position = playerSpawnPos[1].position;
        players[0].Initialize();
        players[1].Initialize();
        ChangeTurn();
    }

    #region Messages
    public void Fire(Vector2 direction, float force) {
        //apply force
        //wait for coin to stop
        placedCoins.Add(currentCoin);
        currentCoin.Shoot(direction, force);
        StartCoroutine(WaitTillCoinStop());
    }
    #endregion

    bool AllCoinStop() {
        if (placedCoins.Count <= 0) {
            return true;
        }
        foreach (BaseCoin coin in placedCoins) {
            if (coin.IsStopped == false) {
                return false;
            }
        }
        return true;
    }

    IEnumerator WaitTillCoinStop() {
        yield return new WaitUntil(AllCoinStop);
        currentCoin.gameObject.layer = gameObject.layer;
        ChangeTurn();
    }

    BaseCoin GetBaseCoin(Vector3 position) {
        var coin = Instantiate<BaseCoin>(baseCoinPrefab, position, Quaternion.identity);
        return coin;
    }
}
