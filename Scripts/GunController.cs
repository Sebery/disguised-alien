using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour {
    [SerializeField] private GunData[] gunsData;

    private Manager manager;
    private GameController gameController;
    private PlayerController playerController;
    private Vector2 direction = new Vector2(1.0f, 0.0f);
    private float angle;

    private void Start() {
        manager = GameObject.FindGameObjectWithTag("Manager").GetComponent<Manager>();
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        playerController = GetComponentInParent<PlayerController>();
    }

    private void Update() {
        if (playerController.Die) return;

        if (!manager.Mobile) {
            direction = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position);
        } else {
            if (gameController.ShootJoystick.Direction != Vector2.zero)
                direction = gameController.ShootJoystick.Direction;
        }

        direction.Normalize();

        ChangeSortingLayer();
        SetAngle();
    }

    private void SetAngle() {
        angle = (Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg * -1.0f) - 180.0f;
        transform.localRotation = Quaternion.Euler(0.0f, 0.0f, angle);
    }

    private void ChangeSortingLayer() {
        if (playerController.IsMovingUp)
            gunsData[playerController.CurrentGun].Sprite.sortingOrder = playerController.SortingOrder - 1;
        else
            gunsData[playerController.CurrentGun].Sprite.sortingOrder = playerController.SortingOrder + 1;
    }

    public void SelectGun() {
        gunsData[playerController.CurrentGun].IsActive = false;
        gunsData[playerController.CurrentGun].ActiveObject = false;

        do {
            ++playerController.CurrentGun;
            playerController.CurrentGun = playerController.CurrentGun >= gunsData.Length ? 0 : playerController.CurrentGun;
        } while (!gunsData[playerController.CurrentGun].Selectable);

        gunsData[playerController.CurrentGun].IsActive = true;
        gunsData[playerController.CurrentGun].ActiveObject = true;
        playerController.Damage = gunsData[playerController.CurrentGun].Damage;
    }


}
