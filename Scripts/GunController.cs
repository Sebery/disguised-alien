using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour {
    [SerializeField] private PlayerController playerController;
    [SerializeField] private int currentGun;
    [SerializeField] private SpriteRenderer[] guns;
    [SerializeField] private float changeLayerRange;

    private Manager manager;
    private Vector2 mousePos;
    private Vector2 dir =  new Vector2(0.0f, -1.0f);
    private float angle;

    public Vector2 Direction { get => dir; }

    private void Start() {
        manager = GameObject.FindGameObjectWithTag("Manager").GetComponent<Manager>();
    }

    private void Update() {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Rotate();
        ChangeSortingLayer();
    }

    private void Rotate() {
        dir.x = manager.Mobile ? (manager.ShootJoystick.Horizontal != 0.0f ? manager.ShootJoystick.Horizontal : dir.x) :
                         (mousePos - (Vector2)transform.position).x;

        dir.y = manager.Mobile ? (manager.ShootJoystick.Vertical != 0.0f ? manager.ShootJoystick.Vertical : dir.y) :
                         (mousePos - (Vector2)transform.position).y;

        angle = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0.0f, 0.0f, (angle + 180.0f) * -1.0f);
    }

    private void ChangeSortingLayer() {
        Vector2 animDirP = playerController.AnimDirection;
        int currentLayerP = playerController.CurrentSortingOrder;

        if (animDirP.y > 0.0f && animDirP.x <= changeLayerRange && animDirP.x >= -changeLayerRange)
            guns[currentGun].sortingOrder = currentLayerP - 1;
        else
            guns[currentGun].sortingOrder = currentLayerP + 1;
    }

}
