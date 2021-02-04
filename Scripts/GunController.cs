using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour {
    [SerializeField] private PlayerController playerController;
    [SerializeField] private int currentGun;
    [SerializeField] private SpriteRenderer[] guns;
    [SerializeField] private float changeLayerRange;
    private Vector2 mousePos;
    private Vector2 dir;
    private float angle;

    private void Update() {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Rotate();
        ChangeSortingLayer();
    }

    private void Rotate() {
        dir = mousePos - (Vector2)transform.position;
        angle = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0.0f, 0.0f, (angle + 180.0f) * -1.0f);
    }

    private void ChangeSortingLayer() {
        Vector2 animDirP = playerController.GetAnimDirection();
        int currentLayerP = playerController.GetCurrentSortingLayer();

        if (animDirP.y > 0.0f && animDirP.x <= changeLayerRange && animDirP.x >= -changeLayerRange)
            guns[currentGun].sortingOrder = currentLayerP - 1;
        else
            guns[currentGun].sortingOrder = currentLayerP + 1;
    }

    //Getters And Setters
    public Vector2 GetDirection() => dir;
}
