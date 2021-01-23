using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Player : MonoBehaviour {
    [Header("Data")]
    public int lives;
    [Header("Components")]
    public Rigidbody2D rb;
    public Animator anim;
    public Camera mainCamera;
    private Vector2 mousePos;
    private GameController gameController;
    [Header("Movement")]
    public float speed;
    private Vector2 velocity = Vector2.zero;
    [Header("Shooting")]
    public Animator gunAnim;
    public Transform GunReference;
    public Transform bulletSpawn;
    public ObjectPoolingController bulletsPool;
    public float delayPerShoot;
    private float gunAngle = 0.0f;
    private Vector2 gunReferenceDir = Vector2.zero;
    private float delayPerShootCounter = 0.0f;
    private bool isShooting = false;
    [Header("Camera")]
    public CinemachineVirtualCamera virtualCamera;
    public float shakeIntensity;
    private CinemachineBasicMultiChannelPerlin channelPerlin;


    private void Start() {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        channelPerlin = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    private void Update() {
        if (!gameController.gameOver) {
            mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);

            RotateGun();
            GetDataToMove();
            Shoot();
            ShootDelay();
            SetDirection();
        }

    }

    private void FixedUpdate() {
        if (!gameController.gameOver) {
            if (velocity.magnitude > 1.0f) velocity.Normalize();
            rb.MovePosition(rb.position + velocity * speed * Time.fixedDeltaTime);
        }

    }

    private void GetDataToMove() {
        velocity.x = Input.GetAxisRaw("Horizontal");
        velocity.y = Input.GetAxisRaw("Vertical");

        anim.SetBool("isMoving", velocity != Vector2.zero);
    }

    private void RotateGun() {
        gunReferenceDir = mousePos - (Vector2)GunReference.position;
        gunAngle = Mathf.Atan2(gunReferenceDir.x, gunReferenceDir.y) * Mathf.Rad2Deg;
        GunReference.rotation = Quaternion.Euler(0.0f, 0.0f, (gunAngle + 180.0f) * -1.0f);
    }

    private bool RotateMinMax(float min, float max, float angle) {
        return (angle >= min && angle <= max);
    }

    private void SetDirection() {
        float dir = 0.5f;

        if (RotateMinMax(45.0f, 135.0f, gunAngle)) {
            dir = 1.0f;
        }

        if (RotateMinMax(-135.0f, -45.0f, gunAngle)) {
            dir = -1.0f;
        }

        if (RotateMinMax(-45.0f, 45.0f, gunAngle)) {
            dir = -0.5f;
        }

        anim.SetFloat("direction", dir);
        gunAnim.SetFloat("direction", dir);
    }

    private void Shoot() {
        if (Input.GetMouseButton(0) && !isShooting && bulletsPool.HasObjectsToPool()) {
            isShooting = true;
            CameraShake();

            GameObject newBullet = bulletsPool.GetPoolPrefab();
            newBullet.transform.position = bulletSpawn.transform.position;
            newBullet.transform.rotation = Quaternion.Euler(0.0f, 0.0f, (gunAngle - 90.0f) * -1.0f);
            Bullet bulletScript = newBullet.GetComponent<Bullet>();
            bulletScript.velocity = gunReferenceDir.normalized;
        }

    }

    private void ShootDelay() {
        if (isShooting) {
            delayPerShootCounter += Time.deltaTime;

            if (delayPerShootCounter >= (delayPerShoot / 2.0f)) {
                channelPerlin.m_AmplitudeGain = 0.0f;
            }

            if (delayPerShootCounter >= delayPerShoot) {
                delayPerShootCounter = 0.0f;
                isShooting = false;
            }
        }
    }

    private void CameraShake() {
        channelPerlin.m_AmplitudeGain = shakeIntensity;
    }

    public void Die() {
        if (lives <= 0 && !gameController.gameOver) { 
            anim.SetBool("die", true);
            gameController.gameOver = true;
        }
    }

}
