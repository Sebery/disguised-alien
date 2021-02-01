using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour {
    [SerializeField]
    private GameObject openedRightCollider;
    [SerializeField]
    private GameObject openedLeftCollider;
    [SerializeField]
    private GameObject closedCollider;

    private Animator anim;

    private void Start() {
        anim = GetComponent<Animator>();
    }

    public void OpenDoor() {
        anim.SetBool("open", true);
    }

    public void EnableColldiers() {
        closedCollider.SetActive(false);
        openedRightCollider.SetActive(true);
        openedLeftCollider.SetActive(true);
    }
}
