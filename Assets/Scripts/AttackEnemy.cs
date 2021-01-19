using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackEnemy : MonoBehaviour {
    [Header("Components")]
    public Player playerScript;

    public void AttackPlayer() {
        --playerScript.lives;
        playerScript.Die();
    }
}
