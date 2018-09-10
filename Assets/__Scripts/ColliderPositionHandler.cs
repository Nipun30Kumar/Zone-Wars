using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderPositionHandler : MonoBehaviour {

    public BoxCollider2D leftWallCollider;
    public BoxCollider2D rightWallCollider;

    public float verticalOffset;

    void OnEnable()
    {
        //Debug.Log("Screen Width : " + Screen.width);
        //Debug.Log("Screen Height : " + Screen.height);

        float worldScreenHeight = Camera.main.orthographicSize * 2.0f;
        float worldScreenWidth = (worldScreenHeight / Screen.height) * Screen.width;

        //Debug.Log("Screen width in world coordinates : " + worldScreenWidth);
        float verticalPosition = worldScreenHeight / 2;
        float sidePosition = worldScreenWidth / 2;

        //Debug.Log("Position : " + sidePosition);
        leftWallCollider.transform.position += new Vector3(-sidePosition, 0f, 0f);
        rightWallCollider.transform.position += new Vector3(sidePosition, 0f, 0f);        
    }
}
