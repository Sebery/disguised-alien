using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    [SerializeField]
    private int lives;
    [SerializeField]
    private float speed;
    [SerializeField]
    private BoxCollider2D mainCollider;
    [SerializeField]
    private BoxCollider2D triggerCollider;
    [SerializeField]
    private float timeToDisappear;

    private Transform scapeTarget;
    private bool scape = false;

    private Rigidbody2D rb;
    private Transform target;
    private Animator anim;
    private SpriteRenderer sprite;
    private GameController gameController;
    private ObjectPoolingController enemiesPool;
    private Enemy enemyScript;

    private Vector2 direction = Vector2.zero;
    private Vector2 velocity = Vector2.zero;
    private int totalLives;
    private int startLayer;
    private bool followTarget = true;

    private void OnEnable() {
        Reset();
    }

    private void Start() {
        scapeTarget = GameObject.FindGameObjectWithTag("Scape").transform;
        enemyScript = GetComponent<Enemy>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        startLayer = sprite.sortingOrder;
        totalLives = lives;
        rb = GetComponent<Rigidbody2D>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        anim = GetComponentInChildren<Animator>();
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        enemiesPool = GameObject.Find("EnemiesPool").GetComponent<ObjectPoolingController>();
    }

    private void Update() {
        if (!gameController.gameOver) {
            if (!scape && gameController.GetMissionCompleted()) {
                scape = true;
                target = scapeTarget;
            }

            direction = target.position - transform.position;
            direction.Normalize();
            velocity = direction;
        }

        if (gameController.gameOver && enemyScript.enabled) {
            rb.simulated = false;
            anim.SetBool("finished", true);
            enemyScript.enabled = false;
        }
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

    private void Die() {
        rb.simulated = false;
        followTarget = false;
        anim.SetBool("die", true);
        mainCollider.enabled = false;
        triggerCollider.enabled = false;
        sprite.sortingOrder = -1;

        Invoke(nameof(ReturnToPool), timeToDisappear);
    }

    private void Reset() {
        if (anim) {
            rb.simulated = true;
            followTarget = true;
            anim.SetBool("die", false);
            mainCollider.enabled = true;
            triggerCollider.enabled = true;
            sprite.sortingOrder = startLayer;
            lives = totalLives;
        }
    }

    private void ReturnToPool() {
        enemiesPool.ReturnPoolPrefab(gameObject);
    }

    //Getters and Setters
    public void SetLives(int lives) {
        this.lives = lives;

        if (this.lives <= 0)
            Die();
    }

    public int GetLives() {
        return lives;
    }


}
