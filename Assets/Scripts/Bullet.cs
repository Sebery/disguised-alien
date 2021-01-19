using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
    private ObjectPoolingController bulletPool;
    [Header("Move Data")]
    public Vector2 velocity;
    public Rigidbody2D rb;
    public float speed;
    [Header("Time To Die Data")]
    public float timeToDie;
    private float timeToDieCount;

    private void Awake() {
        timeToDieCount = 0.0f;
        velocity = Vector2.zero;
    }

    private void Start() {
        bulletPool = GameObject.Find("BulletsPool").GetComponent<ObjectPoolingController>();
    }

    private void Update() {
        Die();
    }

    private void FixedUpdate() {
        rb.velocity = (Vector2)Vector3.Normalize(velocity) * speed * Time.fixedDeltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Enemy") || collision.CompareTag("StaticBlock")) {
            ResetBullet();
        }
    }

    void Die()  {
        timeToDieCount += Time.deltaTime;

        if (timeToDieCount >= timeToDie) {
            velocity.x = 0.0f;
            velocity.y = 0.0f;
            timeToDieCount = 0.0f;
            bulletPool.ReturnPoolPrefab(this.gameObject);
        }
    }

    private void ResetBullet() {
        velocity.x = 0.0f;
        velocity.y = 0.0f;
        timeToDieCount = 0.0f;
        bulletPool.ReturnPoolPrefab(this.gameObject);
    }
}
