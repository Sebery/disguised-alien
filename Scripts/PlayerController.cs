using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SortingGroup))]
public class PlayerController : MonoBehaviour {
    [SerializeField] private float speed;
    [SerializeField] private int lives;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Transform bulletSpawn;
    [SerializeField] private float shootDelay;
    [SerializeField] private Transform gunReference;
    [SerializeField] private Animator[] heartAnims;
    [SerializeField] private float colorDelay;
    [SerializeField] private Text coinsText;
    [SerializeField] private Text potionsText;

    private int currentHearts = 3;
    private int heartValue = 0;
    private SortingGroup _sortingGroup;
    private ObjectPooler bulletPool;
    private Manager manager;
    private GameController gameController;
    private Rigidbody2D _rigidbody2D;
    private Animator _animator;
    private Vector2 moveDirection;
    private Vector2 animDirection;
    private bool isMovingUp = false;
    private int currentGun = 0;
    private bool canShoot = true;
    private bool firstShoot = true;
    private bool die = false;
    private int damage = 1;
    private Color damageColor = new Color(255, 0, 0, 50);
    private Color normalColor = new Color(255, 255, 255, 255);
    private ActivatorController activatorController;
    private int potions = 0;
    private int coins = 0;

    //Animator Parameters
    private const string DIR_X = "DirX";
    private const string DIR_Y = "DirY";
    private const string IS_MOVING = "IsMoving";
    private const string DIE = "Die";
    private const string HEART_STATE = "state";
    private const string ANIMATION = "animation";
   
    public int SortingOrder { get => _spriteRenderer.sortingOrder; }
    public int CurrentGun { get => currentGun; set => currentGun = value; }
    public bool IsMovingUp { get => isMovingUp; }
    public int Damage { get => damage; set => damage = value; }
    public int Potions { 
        get => potions; 

        set {
            potions = value;
            potionsText.text = "Potions: " + potions;
        } 
    }
    public int Coins { 
        get => coins;

        set { 
            coins = value;
            coinsText.text = "Coins: " + coins;
        } 
    }

    public int Lives { 
        get => lives;

        set {
            lives = value;
            //To remove lives
            if (lives <= 0)
                heartAnims[2].SetInteger(HEART_STATE, -1);
            else if (lives <= heartValue)
                heartAnims[1].SetInteger(HEART_STATE, -1);
            else if (lives <= heartValue * 2)
                heartAnims[0].SetInteger(HEART_STATE, -1);

            _spriteRenderer.color = damageColor;
            Invoke(nameof(ResetColor), colorDelay);
        } 
    }
    public bool Die { get => die; }

    private void Start() {
        //lives should be divisible by 3
        heartValue = lives / currentHearts;
        _sortingGroup = GetComponent<SortingGroup>();
        bulletPool = GameObject.FindGameObjectWithTag("BulletPool").GetComponent<ObjectPooler>();
        manager = GameObject.FindGameObjectWithTag("Manager").GetComponent<Manager>();
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponentInChildren<Animator>();
        _animator.SetFloat(DIR_X, 1.0f);
        _animator.SetFloat(DIR_Y, 0.0f);
        animDirection.x = 1.0f;
        animDirection.y = 0.0f;
        activatorController = GameObject.FindGameObjectWithTag("Activator").GetComponent<ActivatorController>();
    }

    private void Update() {
        if (die || activatorController.AnimationStarted) {
            _rigidbody2D.velocity = Vector2.zero;

            if (activatorController.AnimationStarted)
                _animator.SetBool(ANIMATION, true);

            return;
        }

        SelectDirection();
        SelectAnimation();
        Shoot();
        PlayerDie();
    }

    private void FixedUpdate() {
        if (die || activatorController.AnimationStarted) return;

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

    private void PlayerDie() {
        if (lives <= 0) { 
            _animator.SetBool(DIE, true);
            _sortingGroup.sortingLayerName = "Die";
            die = true;
            _rigidbody2D.velocity = Vector2.zero;
            gameController.GameOver = true;
        }
    }

    private void Shoot() {
        if (manager.Mobile && gameController.ShootJoystick.Direction == Vector2.zero)
            firstShoot = true;

        if (!canShoot || !bulletPool.HasObjectsToPool())
            return;

        if (!manager.Mobile && Input.GetMouseButton(0) || gameController.ShootJoystick.Direction != Vector2.zero) {
            canShoot = false;
            Invoke(nameof(ActivateShoot), shootDelay);
            if (manager.Mobile && firstShoot) {
                firstShoot = false;
                return;
            }

            GameObject bullet = bulletPool.GetPoolPrefab();
            bullet.transform.position = bulletSpawn.transform.position;
            bullet.transform.rotation = gunReference.transform.localRotation;
        }
    }

    private void ActivateShoot() {
        canShoot = true;
    }

    private void ResetColor() {
        _spriteRenderer.color = normalColor;
    }


}
