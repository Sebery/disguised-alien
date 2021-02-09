using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour {
    [SerializeField] private float delay;

    private float counter = 0.0f;
    private bool isTimeCompleted = false;
    private bool timing = false;

    public bool IsTimeCompleted { get => isTimeCompleted; set => isTimeCompleted = value; }

    private void Update() {
        if (timing)
            Timing();
    }

    private void Timing() {
        counter += Time.deltaTime;

        if (counter >= delay) {
            timing = false;
            isTimeCompleted = true;
        }
    }

    public void StartTimer() {
        timing = true;
        counter = 0.0f;
        isTimeCompleted = false;
    }

}
