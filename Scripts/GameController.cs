using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {
    [SerializeField] private GameObject mobileCanvas;
    [SerializeField] private Joystick moveJoystick;
    [SerializeField] private Joystick shootJoystick;
    [SerializeField] private int missionIndex;
    [SerializeField] private Text timeText;

    private float currentTime = 0.0f;
    private bool missionCompleted = false;
    private Manager manager;
    private bool gameOver = false;
    private bool attackingPlayer = false;

    public int MissionIndex { get => missionIndex; }
    public bool GameOver { get => gameOver; set => gameOver = value; }
    public Joystick MoveJoystick { get => moveJoystick; }
    public Joystick ShootJoystick { get => shootJoystick; }
    public bool MissionCompleted { get => missionCompleted; set => missionCompleted = value; }
    public bool AttackingPlayer { get => attackingPlayer; set => attackingPlayer = value; }

    private void Start() {
        manager = GameObject.FindGameObjectWithTag("Manager").GetComponent<Manager>();

        if (!manager.Mobile)
            mobileCanvas.SetActive(false);
    }

    private void Update() {
        if (!gameOver) { 
            currentTime += Time.deltaTime;
            timeText.text = "Current Time: " + (int)currentTime + " s";
        }

        
    }
}
