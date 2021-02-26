using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeGun : MonoBehaviour {
    [SerializeField] private GameObject[] gunIcons;

    private PlayerController playerController;

    private void Start() {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    private void DisableIcons() {
        foreach (GameObject obj in gunIcons) {
            obj.SetActive(false);
        }
    }

    public void ShowIcon() {
        DisableIcons();
        gunIcons[playerController.CurrentGun].SetActive(true); ;
    }
}
