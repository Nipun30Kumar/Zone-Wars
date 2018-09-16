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
    
}
