using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {
    [SerializeField]
    private Transform enemiesSpawn;
    private ObjectPoolingController enemiesPool;

    public bool gameOver = false;

    private void Start() {
        enemiesPool = GameObject.Find("EnemiesPool").GetComponent<ObjectPoolingController>();
        //SpawnEnemies();
    }

    private void SpawnEnemies() {
        if (enemiesPool.HasObjectsToPool()) {
            GameObject enemy = enemiesPool.GetPoolPrefab();
            enemy.transform.position = enemiesSpawn.position;
        }

        if (!gameOver)
            Invoke(nameof(SpawnEnemies), 5);
    }
}
