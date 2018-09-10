using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlueRegion : MonoBehaviour {

    public float zoneDragValue;
    public List<GameObject> coin = new List<GameObject>();

    public  void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Coin")
        {
            coin.Add(col.gameObject);
            Rigidbody2D coinRB = col.GetComponent<Rigidbody2D>();
            coinRB.drag += zoneDragValue;
        }
    }

    public void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == "Coin")
        {
            coin.Remove(col.gameObject);
            Rigidbody2D coinRB = col.GetComponent<Rigidbody2D>();
            coinRB.drag -= zoneDragValue;
        }
    }
}
