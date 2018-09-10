using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterDelay : MonoBehaviour {

	void Start()
    {
        Destroy(this.gameObject, 4f);
    }
}
