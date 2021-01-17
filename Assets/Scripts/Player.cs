using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    [Header("Components")]
    public Rigidbody2D rb;
    public Animator anim;
    public Camera mainCamera;
    private Vector2 mousePos;
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

    private void Update() {
        mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);

        RotateGun();
        GetDataToMove();
        Shoot();
        ShootDelay();
    }

    private void FixedUpdate() {
        if (velocity.magnitude > 1.0f) {
            velocity.Normalize();
        }

        rb.MovePosition(rb.position + velocity * speed * Time.fixedDeltaTime);
    }

    private void GetDataToMove() {
        velocity.x = Input.GetAxisRaw("Horizontal");
        velocity.y = Input.GetAxisRaw("Vertical");

        anim.SetBool("isMoving", velocity != Vector2.zero);
    }

    private void RotateGun() {
        gunReferenceDir = mousePos - (Vector2)GunReference.position;
        gunAngle = (Mathf.Atan2(gunReferenceDir.x, gunReferenceDir.y) * Mathf.Rad2Deg);
        if (gunAngle >= 70.0f && gunAngle <= 110.0f)
        {
            gunAnim.SetBool("right", true);
            GunReference.rotation = Quaternion.Euler(0.0f, 0.0f, (gunAngle - 90.0f) * -1.0f);
        }
        else {
            gunAnim.SetBool("right", false);
        }

        if (gunAngle <= -70.0f && gunAngle >= -110.0f)
        {
            gunAnim.SetBool("left", true);
            GunReference.rotation = Quaternion.Euler(0.0f, 0.0f, (gunAngle + 90.0f) * -1.0f);
        }
        else {
            gunAnim.SetBool("left", false);
        }
        
        if(!(gunAngle >= 70.0f && gunAngle <= 110.0f) && !(gunAngle <= -70.0f && gunAngle >= -110.0f)) {
            GunReference.rotation = Quaternion.Euler(0.0f, 0.0f, (gunAngle + 180.0f) * -1.0f);
        }

        //TODO: Add gun idle animation and Make the code better        

    }

    private void Shoot() {
        if (Input.GetMouseButton(0) && !isShooting && bulletsPool.HasObjectsToPool()) {
            isShooting = true;

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

            if (delayPerShootCounter >= delayPerShoot) {
                delayPerShootCounter = 0.0f;
                isShooting = false;
            }
        }
    }

}
