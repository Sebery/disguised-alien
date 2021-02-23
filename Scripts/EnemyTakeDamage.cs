using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class EnemyTakeDamage : MonoBehaviour {
    private EnemyController enemyController;
    private BoxCollider2D _boxCollider2D;
    private PlayerController playerController;

    private void OnEnable() {
        if (!_boxCollider2D) return;

        _boxCollider2D.enabled = true;
    }

    private void Start() {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        enemyController = GetComponentInParent<EnemyController>();
        _boxCollider2D = GetComponent<BoxCollider2D>();
    }

    private void Update() {
        if (enemyController.Die)
            _boxCollider2D.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Bullet"))
            enemyController.Lives -= playerController.Damage;
    }
}
