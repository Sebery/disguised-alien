using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
public class PlayerController : MonoBehaviour {
    [SerializeField] private float speed;
    [SerializeField] private GunController gunController;
    [SerializeField] private Timer shootTimer;
    [SerializeField] private ObjectPooler bulletPool;
    [SerializeField] private Transform bulletSpawn;
    [SerializeField] private Transform gunsRef;
    
    private Manager manager;
    private Vector2 dir;
    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sprite;
    private Vector2 animDir = new Vector2(0.0f, -1.0f);
    //Delay to the first time to shoot
    private bool firstShoot = true;

    public int CurrentSortingOrder { get => sprite.sortingOrder; }
    public Vector2 AnimDirection { get => animDir; }

    private void Start() {
        Init();
    }

    private void Update() {
        CalcMoveDirection();
        CalcAnimDirection();

        Shoot();
    }

    private void FixedUpdate() {
        rb.velocity = speed * Time.deltaTime * dir;
    }

    private void CalcMoveDirection() {
        dir.x = manager.Mobile ? manager.MoveJoystick.Horizontal : Input.GetAxis("Horizontal");
        dir.y = manager.Mobile ? manager.MoveJoystick.Vertical : Input.GetAxis("Vertical");

        dir = (dir.sqrMagnitude > 1.0f) ? dir.normalized : dir;
        anim.SetBool("isMoving", dir != Vector2.zero);
    }

    private void Init() {
        //To be able to shoot the first time
        shootTimer.IsTimeCompleted = true;

        manager = GameObject.FindGameObjectWithTag("Manager").GetComponent<Manager>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
    }

    private void CalcAnimDirection() {
        animDir = gunController.Direction.normalized;
        anim.SetFloat("dirX", animDir.x);
        anim.SetFloat("dirY", animDir.y);
    }

    private void Shoot() {
        if (!manager.Mobile && Input.GetMouseButton(0) ||
            manager.Mobile && manager.ShootJoystick.Horizontal != 0.0f||
            manager.Mobile && manager.ShootJoystick.Vertical != 0.0f) {

            if (!shootTimer.IsTimeCompleted) return;

            shootTimer.StartTimer();
            if (bulletPool.HasObjectsToPool() && !firstShoot) {
                GameObject bullet = bulletPool.GetPoolPrefab();
                bullet.transform.rotation = gunsRef.transform.localRotation;
                bullet.transform.position = bulletSpawn.transform.position;
            }

            firstShoot = false;
        } else {
            firstShoot = true;
        }
    }

}
