using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivatorController : MonoBehaviour {
    private bool animationStarted = false;

    public bool AnimationStarted { get => animationStarted; }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player"))
            StartAnimation();
    }

    private void StartAnimation() {
        animationStarted = true;
    }
}
