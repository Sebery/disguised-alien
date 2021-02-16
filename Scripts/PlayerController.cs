using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour {
    [SerializeField] private float speed;
    [SerializeField] private int lives;
    [SerializeField] private SpriteRenderer _spriteRenderer;

    //Animation Parameters
    private const string DIR_X = "DirX";
    private const string DIR_Y = "DirY";
    private const string IS_MOVING = "IsMoving";
    private const string DIE = "Die";

    private Manager manager;
    private GameController gameController;
    private Rigidbody2D _rigidbody2D;
    private Animator _animator;
    private Vector2 moveDirection;
    private Vector2 animDirection;
    private bool isMovingUp = false;
    private int currentGun = 0;

    public int SortingOrder { get => _spriteRenderer.sortingOrder; }
    public int CurrentGun { get => currentGun; }
    public bool IsMovingUp { get => isMovingUp; }

    private void Start() {
        manager = GameObject.FindGameObjectWithTag("Manager").GetComponent<Manager>();
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponentInChildren<Animator>();
        _animator.SetFloat(DIR_X, 1.0f);
        _animator.SetFloat(DIR_Y, 0.0f);
        animDirection.x = 1.0f;
        animDirection.y = 0.0f;
    }

    private void Update() {
        SelectDirection();
        SelectAnimation();
        Die();
    }

    private void FixedUpdate() {
        _rigidbody2D.velocity = speed * Time.deltaTime * moveDirection;
    }

    private void SelectDirection() {
        if (!manager.Mobile) {
            moveDirection.x = Input.GetAxisRaw("Horizontal");
            moveDirection.y = Input.GetAxisRaw("Vertical");
            animDirection = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - (Vector2)transform.localPosition;
        } else {
            moveDirection.x = gameController.MoveJoystick.Horizontal;
            moveDirection.y = gameController.MoveJoystick.Vertical;

            if (gameController.ShootJoystick.Direction != Vector2.zero)
                animDirection = gameController.ShootJoystick.Direction;
        }

        animDirection.Normalize();
        moveDirection.Normalize();
    }

    private void SelectAnimation() {
        _animator.SetBool(IS_MOVING, moveDirection != Vector2.zero);
        _animator.SetFloat(DIR_X, animDirection.x);
        _animator.SetFloat(DIR_Y, animDirection.y);
        isMovingUp = (animDirection.y > 0.866f); 
    }

    private void Die() {
        if (lives <= 0)
            _animator.SetBool(DIE, true);
    }


}
