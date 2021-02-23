using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class BulletController : MonoBehaviour {
    [SerializeField] private float speed;
    [SerializeField] private Sprite[] bullets;
    [SerializeField] private float timeToDestroy;

    private Manager manager;
    private GameController gameController;
    private GameObject player;
    private PlayerController playerController;
    private SpriteRenderer _spriteRenderer;
    private Rigidbody2D _rigidbody2D;
    private ObjectPooler bulletPool;
    private bool firstTimeEnabled = true;
    private Vector2 direction;

    private void OnEnable() {
        if (firstTimeEnabled) {
            Init();
            firstTimeEnabled = false;
            return;
        }

        Invoke(nameof(DestroyBullet), timeToDestroy);
        SetDirection();
    }

    private void Init() {
        manager = GameObject.FindGameObjectWithTag("Manager").GetComponent<Manager>();
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        player = GameObject.FindGameObjectWithTag("Player");
        bulletPool = GetComponentInParent<ObjectPooler>();
        playerController = player.GetComponent<PlayerController>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _spriteRenderer.sprite = bullets[playerController.CurrentGun];
    }

    private void FixedUpdate() {
        _rigidbody2D.velocity = speed * Time.deltaTime * direction;        
    }

    private void DestroyBullet() {
        bulletPool.ReturnPoolPrefab(gameObject);
    }

    private void SetDirection() {
        if (!manager.Mobile)
            direction = ((Camera.main.ScreenToWorldPoint(Input.mousePosition)) - player.transform.position);
        else
            direction = gameController.ShootJoystick.Direction;
        
        direction.Normalize();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Wall") || collision.CompareTag("Enemy") || collision.CompareTag("StaticEnemy"))
            DestroyBullet();
    }

}
