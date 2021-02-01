using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {
    [SerializeField, Header("Enemies Spawner")]
    private float delay;
    [SerializeField]
    private Transform[] spawns;
    [SerializeField, Header("Level Mission")]
    private GameObject general;
    [SerializeField]
    private GameObject[] generalSpawns;
    [SerializeField]
    private int totalGenerals;

    private ObjectPoolingController enemiesPool;
    public bool gameOver = false;
    private bool missionCompleted = false;

    private void Start() {
        enemiesPool = GameObject.Find("EnemiesPool").GetComponent<ObjectPoolingController>();
        SpawnEnemies();
        SpawnGeneral();
    }

    private void SpawnEnemies() {
        if (!gameOver) {
            if (enemiesPool.HasObjectsToPool()) {
                GameObject enemy = enemiesPool.GetPoolPrefab();
                enemy.transform.position = spawns[Random.Range(0, spawns.Length)].position;
            }

            Invoke(nameof(SpawnEnemies), delay);
        }
    }

    private void SpawnGeneral() {
        int[] random = new int[totalGenerals];

        while (Equality(random)) {
            for (int i = 0; i < random.Length; ++i) {
                random[i] = Random.Range(0, generalSpawns.Length);
            }
        }

        for (int i = 0; i < random.Length; ++i) {
            GameObject obj = Instantiate(general, generalSpawns[random[i]].transform.position, Quaternion.identity);            
            obj.GetComponent<Animator>().SetFloat("direction", generalSpawns[random[i]].transform.localScale.x);
        }
    }

    private bool Equality(int[] numbers) {
        for (int i = 0; i < numbers.Length; ++i) { 
            for (int j = 0; j < numbers.Length; ++j) { 
                if (i != j && numbers[i] == numbers[j])
                    return true;
            }
        }
        return false;
    }

    //Getters and Setters
    public bool GetMissionCompleted() => missionCompleted;
    public void SetMissionCompleted(bool missionCompleted) { 
        this.missionCompleted = missionCompleted;

        if (missionCompleted)
            Debug.Log("Mission Completed!");
    }
}
