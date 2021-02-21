﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.AI;

[RequireComponent(typeof(EnemyAI))]
[RequireComponent(typeof(SortingGroup))]
public class EnemyController : MonoBehaviour {
    [SerializeField] private Animator _animator;
    [SerializeField] private int lives;
    [SerializeField] private int damage;
    [SerializeField] private Timer attackTimer;
    [SerializeField] private Timer dieTimer;

    private PlayerController playerController;
    private GameController gameController;
    private EnemyAI enemyAI;
    private bool die = false;
    private SortingGroup _sortingGroup;
    private ObjectPooler enemiesPool;
    private int totalLives;
    private float totalSpeed;
    private NavMeshAgent agent;

    //Animator Parameters
    private const string DIR_X = "DirX";
    private const string DIR_Y = "DirY";
    private const string DIE = "die";
    private const string ATTACK = "attack";
    private const string GAMEOVER = "gameover";

    public bool Die { get => die; }
    public int Lives { get => lives; set => lives = value; }

    private void OnEnable() {
        if (!agent) return;
        
        ResetEnemy();
    }

    private void Start() {
        totalLives = lives;
        agent = GetComponent<NavMeshAgent>();
        totalSpeed = agent.speed;
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        enemiesPool = GameObject.FindGameObjectWithTag("EnemySpawner").GetComponent<ObjectPooler>();
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        _sortingGroup = GetComponent<SortingGroup>();
        enemyAI = GetComponent<EnemyAI>();

        _sortingGroup.sortingLayerName = "Foreground";
    }

    private void Update() {
        if (gameController.GameOver) {
            attackTimer.CancelTimer();
            _animator.SetBool(GAMEOVER, true);
            return;
        }

        if (dieTimer.TimeCompleted) {
            dieTimer.CancelTimer();
            enemiesPool.ReturnPoolPrefab(gameObject);
        }

        if (!die && lives <= 0)
            EnemyDie();

        if (die) return;

        _animator.SetFloat(DIR_X, enemyAI.Direction.x);
        _animator.SetFloat(DIR_Y, enemyAI.Direction.y);

        if (attackTimer.TimeCompleted)
            playerController.Lives -= damage;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            attackTimer.StartTimer();
            _animator.SetBool(ATTACK, true);
            enemyAI.Stop = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            attackTimer.CancelTimer();
            _animator.SetBool(ATTACK, false);
            enemyAI.Stop = false;
        }
    }

    private void EnemyDie() {
        die = true;
        _animator.SetBool(DIE, true);
        _sortingGroup.sortingLayerName = "Die";
        dieTimer.StartTimer();
    }

    private void ResetEnemy() {
        lives = totalLives;
        die = false;
        agent.speed = totalSpeed;
        _sortingGroup.sortingLayerName = "Foreground";
    }

}