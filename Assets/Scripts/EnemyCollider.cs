using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCollider : MonoBehaviour {
    [SerializeField]
    private Enemy enemy;

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Bullet")) 
            enemy.SetLives(enemy.GetLives() - 1);
        
    }
}
