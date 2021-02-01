using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sargent : MonoBehaviour {
    [SerializeField]
    private int lives;

    private Animator anim;
    private bool die;
    private Mission01 mission01;
    private GameController gameController;

    private void Start() {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        mission01 = GameObject.Find("Mission01").GetComponent<Mission01>();
        die = false;
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Bullet") && !die) {
            --lives;

            if (lives <= 0) {
                gameController.SetMissionCompleted(mission01.SargentDefeated());
                die = true;
                anim.SetBool("die", true);
            }
        }
    }

}
