using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyAI))]
public class EnemyController : MonoBehaviour {
    [SerializeField] private Animator _animator;
    [SerializeField] private int lives;
    

    private EnemyAI enemyAI;
    private bool die = false;
    private SpriteRenderer _spriteRenderer;

    //Animator Parameters
    private const string DIR_X = "DirX";
    private const string DIR_Y = "DirY";
    private const string DIE = "die";
    private const string ATTACK = "attack";

    public bool Die { get => die; }
    public int Lives { get => lives; set => lives = value; }

    private void Start() {
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        enemyAI = GetComponent<EnemyAI>();
    }

    private void Update() {
        if (!die && lives <= 0)
            EnemyDie();

        if (die) return;

        _animator.SetFloat(DIR_X, enemyAI.Direction.x);
        _animator.SetFloat(DIR_Y, enemyAI.Direction.y);
    }

    private void OnTriggerStay2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            _animator.SetBool(ATTACK, true);
            enemyAI.Stop = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.CompareTag("Player")) { 
            _animator.SetBool(ATTACK, false);
            enemyAI.Stop = false;
        }
    }

    private void EnemyDie() {
        die = true;
        _animator.SetBool(DIE, true);
        _spriteRenderer.sortingOrder = -1;
    }


}
