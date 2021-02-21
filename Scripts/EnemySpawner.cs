using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ObjectPooler))]
[RequireComponent(typeof(Timer))]
public class EnemySpawner : MonoBehaviour {
    [SerializeField] private Transform[] enemySpawns;

    private ObjectPooler enemiesPool;
    private GameController gameController;
    private Timer timer;

    private void Start() {
        enemiesPool = GetComponent<ObjectPooler>();
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        timer = GetComponent<Timer>();

        timer.StartTimer();
    }

    private void Update() {
        if (gameController.GameOver) {
            timer.CancelTimer();
            return;
        }

        if (timer.TimeCompleted)
            SpawnEnemy();
    }

    private void SpawnEnemy() {
        if (enemiesPool.HasObjectsToPool()) {
            GameObject enemy = enemiesPool.GetPoolPrefab();
            enemy.transform.position = enemySpawns[Random.Range(0, enemySpawns.Length)].position;
        }
    }

}
