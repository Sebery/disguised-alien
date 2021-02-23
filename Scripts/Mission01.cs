using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mission01 : MonoBehaviour {
    [SerializeField] private GameObject general;
    [SerializeField] private Transform[] generalSpawns;
    [SerializeField] private int totalGenerals;

    private int toReachGoal = 0;
    private const string ROTATION = "rotation";
    private GameController gameController;

    public int ToReachGoal { get => toReachGoal; set => toReachGoal = value; }

    private void Start() {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        SpawnGenerals();
    }

    private void Update() {
        if (!gameController.MissionCompleted) {
            if (toReachGoal >= totalGenerals)
                gameController.MissionCompleted = true;
        }
    }

    private void SpawnGenerals() {
        int[] random = new int[totalGenerals];

        do {
            for (int i = 0; i < random.Length; ++i) {
                random[i] = Random.Range(0, generalSpawns.Length);
            }
        } while (Equality(random));

        for (int i = 0; i < random.Length; ++i) {
            GameObject obj = Instantiate(general, generalSpawns[random[i]].transform.position, Quaternion.identity);
            obj.GetComponent<Animator>().SetFloat(ROTATION, generalSpawns[random[i]].transform.localScale.x);
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

}
