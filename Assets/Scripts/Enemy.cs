using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    public Rigidbody2D rb;
    public Transform target;
    public float speed;
    public SpriteRenderer sprite;
    public Animator anim;
    private Vector2 direction;
    private Vector2 velocity;
    private bool isColliding;

    private void Awake() {
        direction = new Vector2(0.0f, 0.0f);
        velocity = new Vector2(0.0f, 0.0f);
    }

    private void Update() {
        direction = target.position - transform.position;
        direction.Normalize();
        velocity = direction;

        ////if (transform.position.y < target.transform.position.y) {
        ////    sprite.sortingOrder = 5;
        ////} else {
        ////    sprite.sortingOrder = 1;
        ////}

    }

    private void FixedUpdate() {
        if (!isColliding) {
            anim.SetBool("attacking", false);
            rb.MovePosition((Vector2)transform.position + (velocity * speed * Time.fixedDeltaTime));
            anim.SetFloat("horizontal", velocity.x);
            anim.SetFloat("vertical", velocity.y);
        } else {
            anim.SetBool("attacking", true);
            rb.velocity = Vector2.zero;
        }

    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.collider.CompareTag("Player")) {
            isColliding = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision) {
        if (collision.collider.CompareTag("Player")) {
            isColliding = false;
        }
    }

}
