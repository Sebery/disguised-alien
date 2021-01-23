using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    [Header("Data")]
    public int lives;
    [Header("Components")]
    public Rigidbody2D rb;
    public Transform target;
    public SpriteRenderer sprite;
    public Animator anim;
    private GameController gameController;
    [Header("Movement")]
    public float speed;
    private Vector2 direction = Vector2.zero;
    private Vector2 velocity = Vector2.zero;
    private bool isColliding;

    private void Start() {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
    }

    private void Update() {
        if (!gameController.gameOver) {
            direction = target.position - transform.position;
            direction.Normalize();
            velocity = direction;
        }
    }

    private void FixedUpdate() {
        if (!gameController.gameOver) {
            if (!isColliding) {
                anim.SetBool("attacking", false);
                rb.MovePosition((Vector2)transform.position + (velocity * speed * Time.fixedDeltaTime));
                anim.SetFloat("horizontal", velocity.x);
                anim.SetFloat("vertical", velocity.y);
            } else {
                anim.SetBool("attacking", true);
                rb.velocity = Vector2.zero;
            }
        } else {
            rb.velocity = Vector2.zero;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.collider.CompareTag("Player") && !gameController.gameOver)
            isColliding = true;
    }

    private void OnCollisionExit2D(Collision2D collision) {
        if (collision.collider.CompareTag("Player") && !gameController.gameOver)
            isColliding = false;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Bullet") && !gameController.gameOver)
            --lives;

        if (lives <= 0 && !gameController.gameOver)
            Destroy(this.gameObject);
    }

}
