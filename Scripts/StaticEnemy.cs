using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class StaticEnemy : MonoBehaviour {
    [SerializeField] private int lives;

    private const string DIE = "die";

    private PlayerController playerController;
    private Animator _animator;
    private GameController gameController;
    private Mission01 mission01;

    private void Start() {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        _animator = GetComponent<Animator>();
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();

        if (gameController.MissionIndex == 0)  {
            mission01 = GameObject.Find("Mission01").GetComponent<Mission01>();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Bullet")) {
            lives -= playerController.Damage;
            if (lives <= 0)
                Die();
        }
    }

    private void Die() {
        _animator.SetBool(DIE, true);

        if (gameController.MissionIndex == 0) {
            mission01.ToReachGoal += 1;
        }
    }
}
