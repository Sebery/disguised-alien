using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    [SerializeField]
    private int lives;
    [SerializeField]
    private float speed;

    private int totalLives;
    private Rigidbody2D rb;
    private Transform target;
    private Animator anim;
    private GameController gameController;
    private ObjectPoolingController enemiesPool;
    private Vector2 direction = Vector2.zero;
    private Vector2 velocity = Vector2.zero;
    private bool followTarget = true;

    private void Start() {
        totalLives = lives;
        rb = GetComponent<Rigidbody2D>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        anim = GetComponentInChildren<Animator>();
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        enemiesPool = GameObject.Find("EnemiesPool").GetComponent<ObjectPoolingController>();
    }

    private void Update() {
        direction = target.position - transform.position;
        direction.Normalize();
        velocity = direction;

        if (gameController.gameOver)
            GetComponent<Enemy>().enabled = false;
    }

    private void FixedUpdate() {
        if (followTarget) {
            anim.SetBool("attacking", false);
            rb.MovePosition((Vector2)transform.position + (velocity * speed * Time.fixedDeltaTime));
            anim.SetFloat("horizontal", velocity.x);
            anim.SetFloat("vertical", velocity.y);
        } 
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.collider.CompareTag("Player")) {
            anim.SetBool("attacking", true);
            rb.velocity = Vector2.zero;
            followTarget = false;
        }
    }

    private void OnCollisionExit2D(Collision2D collision) {
        if (collision.collider.CompareTag("Player"))
            followTarget = true;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Bullet"))
            --lives;

        if (lives <= 0) {
            Reset();
            enemiesPool.ReturnPoolPrefab(gameObject);
        }
    }

    private void Reset() {
        lives = totalLives;
        followTarget = true;
    }

}
