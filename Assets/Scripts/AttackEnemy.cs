using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackEnemy : MonoBehaviour {
    private Player playerScript;

    private void Start() {
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    public void AttackPlayer() {
        --playerScript.lives;
        playerScript.Die();
    }
}
