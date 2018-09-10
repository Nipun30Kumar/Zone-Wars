using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour {

    public void ResetCollision()
    {
        this.GetComponent<CircleCollider2D>().isTrigger = false;       
    }

}
