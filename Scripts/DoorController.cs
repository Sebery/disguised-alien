using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class DoorController : MonoBehaviour {
    [SerializeField] private GameObject colliderObj;

    private GameController gameController;
    private bool open = false;
    private const string OPEN = "open";
    private Animator _animator;

    private void Start() {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        _animator = GetComponent<Animator>();
    }
        
    private void Update() {
        if (!open && gameController.MissionCompleted) {
            open = true;
            _animator.SetBool(OPEN, true);
        }
    }

    //Animation Event
    public void OpenDoor() {
        colliderObj.SetActive(false);
    }
}
