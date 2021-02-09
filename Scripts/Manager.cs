using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour {
    [SerializeField] private bool mobile;
    [SerializeField] private Joystick shootJoystick;
    [SerializeField] private Joystick moveJoystick;

    public bool Mobile { get => mobile; }
    public Joystick ShootJoystick { get => shootJoystick; }
    public Joystick MoveJoystick { get => moveJoystick; }
}
