using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {
    [SerializeField] private GameObject mobileCanvas;
    [SerializeField] private Joystick moveJoystick;
    [SerializeField] private Joystick shootJoystick;

    private Manager manager;
    private bool gameOver = false;

    public bool GameOver { get => gameOver; set => gameOver = value; }

    public Joystick MoveJoystick { get => moveJoystick; }
    public Joystick ShootJoystick { get => shootJoystick; }

    private void Start() {
        manager = GameObject.FindGameObjectWithTag("Manager").GetComponent<Manager>();

        if (!manager.Mobile)
            mobileCanvas.SetActive(false);
    }
}
