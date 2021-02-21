using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour {
    [SerializeField] private float delay;

    private float time;
    private bool startTimer = false;
    private bool timeCompleted = false;

    public bool TimeCompleted { get => timeCompleted; }

    private void Update() {
        if (startTimer) { 
            time += Time.deltaTime;

            if (time >= delay) {
                time = 0.0f;
                timeCompleted = true;
            } else {
                timeCompleted = false;
            }
        }
    }

    public void StartTimer() {
        startTimer = true;
    }

    public void CancelTimer() {
        startTimer = false;
        timeCompleted = false;
        time = 0.0f;
    }
}
