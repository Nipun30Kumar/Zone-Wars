using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCoin : MonoBehaviour {
    public string playerId;
    public float minPower, maxPower;
  
    new private Rigidbody2D rigidbody2D;

    public bool IsStopped { get; private set; }
    public bool IsHit { get; private set; }

    private Vector2 lastFrameVelocity;

    public void Initialize(string playerId) {
        this.playerId = playerId;
        rigidbody2D = GetComponent<Rigidbody2D>();
        gameObject.SetActive(true);
    }
	
    public void Shoot(Vector2 direction, float force) {
        force = Mathf.Lerp(minPower, maxPower, force);
        rigidbody2D.AddForce(direction * force, ForceMode2D.Force);
        LeanTween.delayedCall(gameObject, 0.1f, () => {
            IsHit = true;
        });
    }

    void Update() {
        lastFrameVelocity = rigidbody2D.velocity;
        if(IsHit) {
            if (rigidbody2D.velocity.magnitude <= 0.1f) {
                IsStopped = true;
                rigidbody2D.Sleep();
            }
        }
    }

    

    private void OnCollisionEnter2D(Collision2D collision) {
        if(collision.transform.tag == "Wall") {
            Bouns(collision.contacts[0].normal);
        } else if(collision.transform.tag == transform.tag) {
            IsHit = true;
            IsStopped = false;
        }
    }

    private void Bouns(Vector2 collisionNormal) {
        var speed = lastFrameVelocity.magnitude;
        var direction = Vector3.Reflect(lastFrameVelocity.normalized, collisionNormal);
        rigidbody2D.velocity = direction * speed;
    }
}
