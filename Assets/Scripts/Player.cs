using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    [Header("Player Data")]
    public Rigidbody2D rb;
    public Animator anim;
    public float speed;
    private Vector2 velocity;
    [Header("Shoot Data")]
    public Transform bulletSpawn;
    public float delayPerShoot;
    public ObjectPoolingController bulletsPool;
    private float delayPerShootCounter;
    private bool isShooting;
    private Object bullet;
    private Vector2 lastPlayerSpeed;

    private void Awake() {
        isShooting = false;
        delayPerShootCounter = 0.0f;
        velocity = new Vector2(0.0f, 0.0f);
        lastPlayerSpeed = new Vector2(0.0f, -1.0f);
        bullet = Resources.Load("BasicGunBullet");
    }

    private void Update() {
        GetDataToMove();
        Shoot();
        ShootDelay();
    }

    private void FixedUpdate() {
        rb.MovePosition(rb.position + (Vector2)Vector3.Normalize(velocity) * speed * Time.fixedDeltaTime);
    }

    private void GetDataToMove() {
        velocity.x = Input.GetAxisRaw("Horizontal");
        velocity.y = Input.GetAxisRaw("Vertical");

        anim.SetFloat("horizontal", velocity.x);
        anim.SetFloat("vertical", velocity.y);
        anim.SetFloat("speed", Vector2.SqrMagnitude(velocity));

        if (velocity != Vector2.zero) { 
            anim.SetFloat("lastspeed", ((velocity.x != 0.0f) ? velocity.x : (velocity.y > 0.0f) ? 2.0f : 0.0f));
            lastPlayerSpeed.x = velocity.x;
            lastPlayerSpeed.y = velocity.y;
        }
    }

    private void Shoot() {
        if (Input.GetKey(KeyCode.Space) && !isShooting) {
            isShooting = true;

            if (bulletsPool.HasObjectsToPool()) {
                GameObject newBullet = bulletsPool.GetPoolPrefab();
                newBullet.transform.position = bulletSpawn.transform.position;
                newBullet.transform.rotation = bulletSpawn.transform.localRotation;
                Bullet bulletScript = newBullet.GetComponent<Bullet>();

                if (velocity == Vector2.zero) {
                    if (lastPlayerSpeed.x != 0.0 && lastPlayerSpeed.y != 0.0f) {
                        bulletScript.velocity.x = lastPlayerSpeed.x;
                    } else {
                        bulletScript.velocity.x = lastPlayerSpeed.x;
                        bulletScript.velocity.y = lastPlayerSpeed.y;
                    }

                } else {
                    bulletScript.velocity = velocity;
                }
            }
 
        }
            
    }

    private void ShootDelay() {
        if (isShooting) {
            delayPerShootCounter += Time.deltaTime;

            if (delayPerShootCounter >= delayPerShoot) {
                delayPerShootCounter = 0.0f;
                isShooting = false;
            }
        }
    }

}
