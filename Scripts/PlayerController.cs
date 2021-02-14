using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour {
    [SerializeField] private float speed;
    [SerializeField] private int lives;

    //Animation Parameters
    private const string LASTDIRX = "LastDirX";
    private const string LASTDIRY = "LastDirY";
    private const string DIRX = "DirX";
    private const string DIRY = "DirY";
    private const string ISMOVING = "IsMoving";
    private const string DIE = "Die";

    private float lastDirX;
    private float lastDirY;
    private Manager manager;
    private GameController gameController;
    private Rigidbody2D _rigidbody2D;
    private Animator _animator;
    private Vector2 direction = Vector2.zero;

    private void Start() {
        lastDirX = 1.0f;
        lastDirY = 0.0f;
        manager = GameObject.FindGameObjectWithTag("Manager").GetComponent<Manager>();
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponentInChildren<Animator>();
    }

    private void Update() {
        SelectDirection();
        SelectAnimation();
        Die();
    }

    private void FixedUpdate() {
        _rigidbody2D.velocity = speed * Time.deltaTime * direction;
    }

    private void SelectDirection() {
        if (!manager.Mobile) {
            direction.x = Input.GetAxisRaw("Horizontal");
            direction.y = Input.GetAxisRaw("Vertical");
            direction.Normalize();
        } else {
            direction.x = gameController.MoveJoystick.Horizontal;
            direction.y = gameController.MoveJoystick.Vertical;
        }
    }

    private void SelectAnimation() {
        if (direction == Vector2.zero) {
            _animator.SetFloat(LASTDIRX, lastDirX);
            _animator.SetFloat(LASTDIRY, lastDirY);
            _animator.SetBool(ISMOVING, false);
        } else {
            lastDirX = direction.x;
            lastDirY = direction.y;
            _animator.SetFloat(DIRX, direction.x);
            _animator.SetFloat(DIRY, direction.y);
            _animator.SetBool(ISMOVING, true);
        }
    }

    private void Die() {
        if (lives <= 0)
            _animator.SetBool(DIE, true);
    }
}
