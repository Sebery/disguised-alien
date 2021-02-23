using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {
    [SerializeField] private GameObject mobileCanvas;
    [SerializeField] private Joystick moveJoystick;
    [SerializeField] private Joystick shootJoystick;
    [SerializeField] private int missionIndex;

    private bool missionCompleted = false;
    private Manager manager;
    private bool gameOver = false;

    public int MissionIndex { get => missionIndex; }
    public bool GameOver { get => gameOver; set => gameOver = value; }
    public Joystick MoveJoystick { get => moveJoystick; }
    public Joystick ShootJoystick { get => shootJoystick; }
    public bool MissionCompleted { get => missionCompleted; set => missionCompleted = value; }

    private void Start() {
        manager = GameObject.FindGameObjectWithTag("Manager").GetComponent<Manager>();

        if (!manager.Mobile)
            mobileCanvas.SetActive(false);
    }
}
